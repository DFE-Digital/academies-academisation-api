using System.Text.Json.Serialization;
using System.Text.Json;

namespace Dfe.Academies.Academisation.SubcutaneousTest.Utils
{
	internal static class HttpResponseMessageExtensions
	{ 
		private static readonly JsonSerializerOptions Options = new()
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		internal static async Task<T> ConvertResponseToTypeAsync<T>(this HttpResponseMessage httpResponseMessage)
		{
			var content = await httpResponseMessage.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(content))
			{
				return default!;
			}
			return JsonSerializer.Deserialize<T>(content, Options)!;
		}
	}
}
