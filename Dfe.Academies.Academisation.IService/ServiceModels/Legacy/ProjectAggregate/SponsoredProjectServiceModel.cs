namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class SponsoredProjectServiceModel
	{
		public SponsoredProjectSchoolServiceModel? School { get; init; }
		public SponsoredProjectTrustServiceModel? Trust { get; init; }
	}

	public class SponsoredProjectTrustServiceModel
	{
		public string? Name { get; init; }
		public string? ReferenceNumber { get; init; }
	}

	public class SponsoredProjectSchoolServiceModel
	{
		public string? Name { get; init; }
		public int Urn { get; init; }
		public DateTime OpeningDate { get; init; }
		public bool PartOfPfiScheme { get; init; }
		public string? LocalAuthorityName { get; init; }
		public string? Region{ get; init; }
	}
}
