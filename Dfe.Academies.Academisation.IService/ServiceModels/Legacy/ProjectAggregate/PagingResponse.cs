namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class PagingResponse
	{
		public int Page { get; set; }
		public int RecordCount { get; set; }
		public string? NextPageUrl { get; set; }
	}
}
