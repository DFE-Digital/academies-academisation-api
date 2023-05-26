using System.Diagnostics;
using Dfe.Academies.Academisation.WebApi.Client;
using Microsoft.Extensions.Configuration;

using var httpClient = new HttpClient();

var config = new ConfigurationBuilder()
	.AddUserSecrets<Program>()
	.Build();

var baseUrl = config.GetSection("settings")["baseUrl"];
var apiKey = config.GetSection("settings")["apiKey"];
httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

var apiClient = new Dfe.Academies.Academisation.WebApi.Client.WebApiClient(baseUrl, httpClient);

Console.WriteLine("Press return to send request");
Console.ReadLine();

// Get all applications
var results = await apiClient.AllAsync(CancellationToken.None);

Console.WriteLine("returned...");
foreach (var result in results)
{
	Console.WriteLine($"Id: {result.Id}reference: {result.ApplicationReference}");
}

Console.WriteLine();
Console.WriteLine("Press return to exit");
Console.ReadLine();
