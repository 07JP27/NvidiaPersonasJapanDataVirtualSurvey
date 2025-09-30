using System.ClientModel;
using Azure.AI.OpenAI;
using NvidiaPersonasJapanDataVirtualSurvey.Core;
using NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

void OnProgressChanged(string message)
{
    Console.WriteLine(message);
}

const string endpoint = "https://xxxxx.openai.azure.com";
const string apiKey = "xxxxxxxxxxxxxxxxxxxxx";
const string deploymentName = "xxxxxxxxxxxxxx";


var aoaiClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey)
);

#pragma warning disable OPENAI001 
var batchClient = aoaiClient.GetBatchClient();
#pragma warning restore OPENAI001 
var fileClient = aoaiClient.GetOpenAIFileClient();

var service = await SurveyService.CreateAsync(batchClient, fileClient, deploymentName, 5, new Progress<string>(OnProgressChanged));

// var request = new FreeTextSurveyRequest("昨今のAI(LLM)の目覚ましい進化についてどう思いますか？", 300);

// var request = new YesNoSurveyRequest("現在の日本において金融緩和政策は必要だと思いますか？");

var request = new OptionSelectSurveyRequest(
    "あなたが食べて見たいのはどちらですか？",
    new List<string> { "正統派芋煮", "庄内風芋煮" },
    isMultiSelect: false
);

var result = await service.RunSurveyAsync(request);


Console.WriteLine("=== Survey Results ===");
Console.WriteLine($"Total Prompt Tokens: {result.Usage.PromptTokens}");
Console.WriteLine($"Total Completion Tokens: {result.Usage.CompletionTokens}");
Console.WriteLine($"Total Tokens: {result.Usage.TotalTokens}");
Console.WriteLine();
foreach (var answer in result.Answers)
{
    Console.WriteLine($"{answer.Persona.Age}歳 / {answer.Persona.Sex} / {answer.Persona.Occupation} / {answer.Persona.Prefecture} 在住");
    Console.WriteLine($"Answer: {answer.Answer}");
    Console.WriteLine("----------------------");
}
Console.WriteLine("========================");
Console.ReadLine();