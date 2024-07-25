namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupResponseModel(string urn, string trustUrn, IEnumerable<ConversionsResponseModel> conversions)
	{
		public string TrustUrn { get; private set; } = trustUrn;

		public string Urn { get; private set; } = urn;

		public IEnumerable<ConversionsResponseModel> Conversions { get; set; } = conversions;
	}

	public class ConversionsResponseModel(int urn, string? schoolName)
	{
		public int Urn { get; private set; } = urn;
		public string? SchoolName { get; private set; } = schoolName;
	}
}
