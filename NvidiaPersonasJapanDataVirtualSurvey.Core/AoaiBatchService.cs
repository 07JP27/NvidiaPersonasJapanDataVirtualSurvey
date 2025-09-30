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
internal class AoaiBatchService(BatchClient batchClient, OpenAIFileClient fileClient, IProgress<string>? progress = null)
{
    internal async Task<string> UploadBatchInputAsync(BatchRequestItem[] payload)
    {
        string jsonlContent = string.Join("\n", payload.Select(item => JsonSerializer.Serialize(item)));
        using MemoryStream fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonlContent));

        OpenAIFile uploadedFile = await fileClient.UploadFileAsync(
            fileStream,
            "batch_input.jsonl",
            FileUploadPurpose.Batch
        );

        return uploadedFile.Id;
    }

    internal async Task<List<BatchOutputItem>> GetBatchResultsAsync(string fileId)
    {
        BinaryData outputContent = await fileClient.DownloadFileAsync(fileId);

        string jsonlOutput = outputContent.ToString();
        string[] lines = jsonlOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        return lines
            .Select(line => JsonSerializer.Deserialize<BatchOutputItem>(line))
            .Where(item => item != null)
            .ToList()!;
    }


    internal async Task<CreateBatchOperation> CreateBatchJobAsync(string fileId)
    {
        var batchRequest = new
        {
            input_file_id = fileId,
            endpoint = "/chat/completions",
            completion_window = "24h",
            output_expires_after = new
            {
                seconds = 1209600
            },
            anchor = "created_at"
        };

        BinaryContent content = BinaryContent.Create(
            BinaryData.FromObjectAsJson(batchRequest)
        );


        CreateBatchOperation result = await batchClient.CreateBatchAsync(content, false);
        return result;
    }

    internal async Task<BatchStatus> WaitForCompleteBatchJobAsync(CreateBatchOperation operation)
    {
        BatchStatus currentStatus = new BatchStatus();

        while (currentStatus.Status != "completed" &&
            currentStatus.Status != "failed" &&
            currentStatus.Status != "cancelled" &&
            currentStatus.Status != "expired")
        {
            ClientResult statusResult = await operation.GetBatchAsync(null);
            BinaryData statusData = statusResult.GetRawResponse().Content;

            currentStatus = JsonSerializer.Deserialize<BatchStatus>(statusData) ?? throw new Exception("Failed to deserialize batch status.");

            if (currentStatus.RequestCounts != null)
            {
                var counts = currentStatus.RequestCounts;
                int total = counts.Total ?? 0;
                int completed = counts.Completed ?? 0;
                int failed = counts.Failed ?? 0;

                progress?.Report($"[{DateTime.Now:HH:mm:ss}] Status: {currentStatus.Status}");
                progress?.Report($"  Progress: {completed}/{total} completed, {failed} failed");
            }
            else
            {
                progress?.Report($"[{DateTime.Now:HH:mm:ss}] Status: {currentStatus.Status}");
            }
            await Task.Delay(TimeSpan.FromSeconds(60));
        }

        return currentStatus;
    }
}
#pragma warning restore OPENAI001