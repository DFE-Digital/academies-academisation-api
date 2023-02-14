namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class InvoluntaryProjectServiceModel
	{
		public SchoolServiceModel? School { get; init; }
		public TrustServiceModel? Trust { get; init; }
	}

	public class TrustServiceModel
	{
		public string? Name { get; init; }
		public string? ReferenceNumber { get; init; }
	}

	public class SchoolServiceModel
	{
		public string? Name { get; init; }
		public int Urn { get; init; }
		public DateTime OpeningDate { get; init; }
		public bool PartOfPfiScheme { get; init; }
	}
}
