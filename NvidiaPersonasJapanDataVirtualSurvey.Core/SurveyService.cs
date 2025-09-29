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

namespace NvidiaPersonasJapanDataVirtualSurvey.Core;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class SurveyService(BatchClient client, IProgress<string>? progress = null)
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
{
    public async Task RunSurveyAsync()
    {
        //var loader = new DataLoader(progress);
        //var list = await loader.LoadAsync(20);


        var inputLines = new object[]
        {
            new {
                custom_id = "task-1",
                method = "POST",
                url = "/chat/completions",
                body = new {
                    model = "gpt-4.1-batch", // デプロイ名
                    messages = new object[] {
                        new { role = "system", content = "You are a helpful assistant. Answer in Japanese." },
                        new { role = "user",   content = "Azure OpenAIのバッチ推論の特徴を一文で説明して。" }
                    }
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(inputLines);
        var binaryContent = BinaryContent.Create(BinaryData.FromString(jsonContent));
        var result = await client.CreateBatchAsync(binaryContent, waitUntilCompleted: true);
        ;
    }
}