using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IntegrationTest.Extensions;

public static class HttpClientExtensions
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		Converters = { new JsonStringEnumConverter() },
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};

	public static async Task<(HttpStatusCode StatusCode, T? Result)> PostAsJsonDeserialized<T>(this HttpClient client, string url, object? value)
	where T: class
	{
		return await client
			.PostAsJsonAsync(url, value)
			.ReadAsDeserialized<T>();
	}
	
	public static async Task<(HttpStatusCode StatusCode, T? Result)> GetDeserialized<T>(this HttpClient client, string url)
		where T: class
	{
		return await client
			.GetAsync(url)
			.ReadAsDeserialized<T>();
	}

	private static async Task<(HttpStatusCode, T?)> ReadAsDeserialized<T>(this Task<HttpResponseMessage> task) 
	where T: class
	{
		var response = await task;

		if (!response.IsSuccessStatusCode) return (response.StatusCode, null);
		
		var json = await response.Content.ReadAsStringAsync();
		
		if (string.IsNullOrWhiteSpace(json)) return (response.StatusCode, null);

		var result = JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);

		return (response.StatusCode, result);
	}
}
