namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class ExportedTransferProjectModel
	{
		public int Id { get; init; }

		public string? AcademyRoute { get; init; }
		public string? AcademyType { get; init; }
		public string? AcademyTypeAndRoute { get; init; }
		public string? AssignedUserFullName { get; init; }
		public DateTime? AdvisoryBoardDate { get; init; }
		public DateTime? DecisionDate { get; init; }
		public string? IncomingTrustName { get; init; }
		public string? IncomingTrustUkprn { get; init; }
		public string? LocalAuthority { get; init; }
		public string? OutgoingTrustName { get; init; }
		public string? OutgoingTrustUKPRN { get; init; }
		public DateTime? ProposedAcademyTransferDate { get; init; }
		public string? Region { get; init; }
		public string? SchoolName { get; init; }
		public string? SchoolType { get; init; }
		public string? Status { get; init; }
		public string? TransferReason { get; init; }
		public string? TransferType { get; init; }
		public string? Urn { get; init; }
		public string? PFI { get; init; }
	}
}
