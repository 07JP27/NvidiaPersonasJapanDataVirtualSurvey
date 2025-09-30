using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ClientModel;
using Azure.AI.OpenAI;
using OpenAI.Batch;
using OpenAI.Chat;
using Parquet;
using Parquet.Data;
using OpenAI.Files;
using NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core;

#pragma warning disable OPENAI001 
public class SurveyService
{
    private readonly BatchClient batchClient;
    private readonly OpenAIFileClient fileClient;
    private readonly string batchDeploymentName;
    private readonly IProgress<string>? progress;

    private List<PersonaRecord> personaList;

    private SurveyService(BatchClient batchClient, OpenAIFileClient fileClient, string batchDeploymentName, IProgress<string>? progress, List<PersonaRecord> personaList)
    {
        this.batchClient = batchClient;
        this.fileClient = fileClient;
        this.batchDeploymentName = batchDeploymentName;
        this.progress = progress;
        this.personaList = personaList;
    }

    public static async Task<SurveyService> CreateAsync(BatchClient batchClient, OpenAIFileClient fileClient, string batchDeploymentName, int SampleSize = 20, IProgress<string>? progress = null)
    {
        var loader = new DataLoader(progress);
        var personaList = await loader.LoadAsync(SampleSize);
        
        return new SurveyService(batchClient, fileClient, batchDeploymentName, progress, personaList.ToList());
    }

    public async Task<SurveyResponse> RunSurveyAsync(ISurveyRequest request)
    {
        List<BatchRequestItem> payloads = new List<BatchRequestItem>();

        foreach (var persona in personaList)
        {
            payloads.Add(
                new BatchRequestItem(
                    CustomId: persona.Uuid.ToString(), //ペルソナのIDとバッチタスクのIDを紐づける
                    Body: new BatchRequestBody(
                        Model: batchDeploymentName,
                        Messages:
                        [
                            new Models.ChatMessage("system", request.GetSystemPrompt(persona)),
                            new Models.ChatMessage("user", request.GetUserPrompt())
                        ]
                    )
                )
            );
        }

        AoaiBatchService batchService = new AoaiBatchService(batchClient, fileClient, progress);

        var uploadedDataId = await batchService.UploadBatchInputAsync(payloads.ToArray());
        var operation = await batchService.CreateBatchJobAsync(uploadedDataId);
        var status = await batchService.WaitForCompleteBatchJobAsync(operation);
        var results = await batchService.GetBatchResultsAsync(status.OutputFileId!);

        SurveyResponse response = new SurveyResponse();

        foreach (var persona in personaList)
        {
            var result = results.FirstOrDefault(r => r.CustomId == persona.Uuid.ToString());
            if (result != null)
            {
                if (result.Response?.Body?.Choices != null && result.Response.Body.Choices.Length > 0)
                {
                    response.Answers.Add(new PersonaAnswer(persona, result.Response.Body.Choices[0].Message?.Content ?? ""));
                    response.Usage.CompletionTokens += result.Response.Body.Usage?.CompletionTokens ?? 0;
                    response.Usage.PromptTokens += result.Response.Body.Usage?.PromptTokens ?? 0;
                }
            }
        }
        return response;
    }
}
#pragma warning restore OPENAI001