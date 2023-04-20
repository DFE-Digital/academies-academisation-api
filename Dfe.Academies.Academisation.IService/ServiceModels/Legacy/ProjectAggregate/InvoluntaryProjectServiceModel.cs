namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class InvoluntaryProjectServiceModel
	{
		public InvoluntaryProjectSchoolServiceModel? School { get; init; }
		public InvoluntaryProjectTrustServiceModel? Trust { get; init; }
	}

	public class InvoluntaryProjectTrustServiceModel
	{
		public string? Name { get; init; }
		public string? ReferenceNumber { get; init; }
	}

	public class InvoluntaryProjectSchoolServiceModel
	{
		public string? Name { get; init; }
		public int Urn { get; init; }
		public DateTime OpeningDate { get; init; }
		public bool PartOfPfiScheme { get; init; }
	}
}
