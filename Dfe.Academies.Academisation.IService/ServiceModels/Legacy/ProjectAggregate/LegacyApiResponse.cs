namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class LegacyApiResponse<TResponse> where TResponse : class
	{
		public IEnumerable<TResponse> Data { get; }
		public LegacyResponse Legacy { get; }
		
		public LegacyApiResponse(IEnumerable<TResponse> data, LegacyResponse legacyResponse)
		{
			Data = data;
			Legacy = legacyResponse;
		}
	}
}
