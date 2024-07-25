
namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupSearchModel(int page, int count, string? urn, string? trustUrn, string? trustName, string? academyName, string? academyUkprn, string? companiesHouseNo)
	{
		public int Page { get; set; } = page;
		public int Count { get; set; } = count;
		public string? Urn { get; set; } = urn;
		public string? TrustUrn { get; set; } = trustUrn;
		public string? TrustName { get; set; } = trustName;
		public string? AcademyName { get; set; } = academyName;
		public string? AcademyUkprn { get; set; } = academyUkprn;
		public string? CompaniesHouseNo { get; set; } = companiesHouseNo;

	}
}
