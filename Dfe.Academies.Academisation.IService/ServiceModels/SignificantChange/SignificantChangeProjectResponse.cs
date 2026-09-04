namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeProjectResponse
	{
		public int Id { get; set; }
		public int Urn { get; set; }
		public byte Tier { get; set; }
		public required string SchoolName { get; set; }
		public required string TrustName { get; set; }
		public required string TrustUkprn { get; set; }
		public required string TypeOfSignificantChange { get; set; }
		public required string Status { get; set; }
		public string? LocalAuthorityName { get; set; }
		public string? CompaniesHouseNumber { get; set; }
	}
}
