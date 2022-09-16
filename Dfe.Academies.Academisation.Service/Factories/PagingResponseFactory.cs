using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Factories;

internal class PagingResponseFactory
{
	internal static PagingResponse Create(string path, int page, int count, int recordCount, 
		Dictionary<string, object?> routeValues)
	{
		var pagingResponse = new PagingResponse { RecordCount = recordCount, Page = page };

		if (recordCount != count) return pagingResponse;
		
		pagingResponse.NextPageUrl = $"{path}?page={page + 1}&count={count}";

		foreach (var routeValue in routeValues)
		{
			if (!string.IsNullOrWhiteSpace(routeValue.Value?.ToString()))
			{
				pagingResponse.NextPageUrl += $"&{routeValue.Key}={routeValue.Value}";
			}	
		}
		return pagingResponse;
	}
}
