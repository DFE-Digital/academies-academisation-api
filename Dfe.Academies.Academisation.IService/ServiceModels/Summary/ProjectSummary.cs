namespace Dfe.Academies.Academisation.IService.ServiceModels.Summary
{
	public class ProjectSummary
	{
		public int Id { get; set; }
		public int Urn { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? LastModifiedOn { get; set; }
		public ConversionsSummary? ConversionsSummary { get; set; }
		public TransfersSummary? TransfersSummary { get; set; }
	}

    public class TransfersSummary
    {
        public string? ProjectReference { get; set; }
        public string? OutgoingTrustUkprn { get; set; }
        public string? OutgoingTrustName { get; set; }
        public string? TypeOfTransfer { get; set; }
        public DateTime? TargetDateForTransfer { get; set; }
        public string? AssignedUserEmailAddress { get; set; }
        public string? AssignedUserFullName { get; set; }
        public string? Status { get; set; }
        public string? IncomingTrustUkprn { get; set; }
        public string? IncomingTrustName { get; set; }
	}

 
	public class ConversionsSummary
	{
		public string? ApplicationReferenceNumber { get; set; }
		public string? SchoolName { get; set; }
		public string? LocalAuthority { get; set; }
		public string? Region { get; set; }
		public string? AcademyTypeAndRoute { get; set; }
		public string? NameOfTrust { get; set; }
		public string? AssignedUserEmailAddress { get; set; }
		public string? AssignedUserFullName { get; set; }
		public string? ProjectStatus { get; set; }
		public string? TrustReferenceNumber { get; set; }
		public DateTime? CreatedOn { get; set; }
		public string? Decision { get; set; }
		public DateTime? ConversionTransferDate { get; set; }
		public string? Route { get; set; }
	}
}
