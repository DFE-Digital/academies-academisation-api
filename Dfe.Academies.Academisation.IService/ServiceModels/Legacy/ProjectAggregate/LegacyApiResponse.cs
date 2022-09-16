namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class LegacyApiResponse<TResponse> where TResponse : class
	{
		public IEnumerable<TResponse> Data { get; }
		public PagingResponse Paging { get; }
		
		public LegacyApiResponse(IEnumerable<TResponse> data, PagingResponse pagingResponse)
		{
			Data = data;
			Paging = pagingResponse;
		}
	}
}
