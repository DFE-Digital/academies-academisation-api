namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupResponseModel(string referenceNumber, string trustReferenceNumber, IEnumerable<ConversionsResponseModel> conversions)
	{
		public string TrustReferenceNumber { get; private set; } = trustReferenceNumber;

		public string GroupReferenceNumber { get; private set; } = referenceNumber;

		public IEnumerable<ConversionsResponseModel> Conversions { get; set; } = conversions;
	}

	public class ConversionsResponseModel(int urn, string? schoolName)
	{
		public int Urn { get; private set; } = urn;
		public string? SchoolName { get; private set; } = schoolName;
	}
}
