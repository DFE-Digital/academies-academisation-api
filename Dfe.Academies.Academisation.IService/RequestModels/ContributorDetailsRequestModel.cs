namespace Dfe.Academies.Academisation.IService.RequestModels
{
	public class ContributorDetailsRequestModel
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? EmailAddress { get; set; }
		public int Role { get; set; }
		public string? OtherRoleName { get; set; }
	}
}
