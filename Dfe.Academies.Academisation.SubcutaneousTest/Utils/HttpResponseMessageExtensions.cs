using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

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

		private static readonly JsonSerializerOptions EnumOptions = new()
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true,
			Converters = { new JsonStringEnumConverter() }
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

		internal static async Task<T> ConvertResponseToEnumTypeAsync<T>(this HttpResponseMessage httpResponseMessage)
		{
			var content = await httpResponseMessage.Content.ReadFromJsonAsync<T>(EnumOptions);
			if (content == null)
			{
				return default!;
			}
			return content;
		}

		internal static async Task<PagedDataResponse<T>> ConvertResponseToPagedDataResponseAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class
		{
			var content = await httpResponseMessage.Content.ReadAsStringAsync();
			if (content == null)
			{
				return default!;
			}
			var jsonDocument = JsonDocument.Parse(content);

			var data = JsonSerializer.Deserialize<IEnumerable<T>>(
			jsonDocument.RootElement.GetProperty("data").GetRawText(), EnumOptions);

			var paging = JsonSerializer.Deserialize<PagingResponse>(jsonDocument.RootElement.GetProperty("paging").GetRawText(), EnumOptions);
			return new PagedDataResponse<T>(data ?? [], paging ?? new PagingResponse());
		}
	}
}
