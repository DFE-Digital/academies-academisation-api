
namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupSearchModel(int page, int count, string? referenceNumber, string? trustReference, string? title)
	{
		public int Page { get; set; } = page;
		public int Count { get; set; } = count;
		public string? ReferenceNumber { get; set; } = referenceNumber;
		public string? TrustReference { get; set; } = trustReference;
		public string? Title { get; set; } = title;

	}
}
