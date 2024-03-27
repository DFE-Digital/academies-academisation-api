namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class NewProjectServiceModel
	{
		public NewProjectSchoolServiceModel? School { get; init; }
		public NewProjectTrustServiceModel? Trust { get; init; }
		public string? HasSchoolApplied { get; init; }
		public string? HasPreferredTrust { get; init; }
		public bool IsFormAMat { get; init; }
	}

	public class NewProjectTrustServiceModel
	{
		public string? Name { get; init; }
		public string? ReferenceNumber { get; init; }
	}

	public class NewProjectSchoolServiceModel
	{
		public string? Name { get; init; }
		public int Urn { get; init; }
		public DateTime ProposedAcademyOpeningDate { get; init; }
		public bool PartOfPfiScheme { get; init; }
		public string? LocalAuthorityName { get; init; }
		public string? Region { get; init; }
	}
}
