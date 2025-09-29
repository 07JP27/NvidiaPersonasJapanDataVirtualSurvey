// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

void OnProgressChanged(string message)
{
    Console.WriteLine(message);
}

var loader = new NvidiaPersonasJapanDataVirtualSurvey.Core.DataLoader(new Progress<string>(OnProgressChanged));
await loader.LoadAsync();