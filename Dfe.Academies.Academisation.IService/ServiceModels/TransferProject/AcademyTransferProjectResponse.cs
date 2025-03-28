
namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class AcademyTransferProjectResponse
    {
		public int Id { get; set; }
        public string? ProjectUrn { get; set; }
		public bool IsReadOnly { get; set; }
        public string? ProjectReference { get; set; }
        public string? OutgoingTrustUkprn { get; set; }
        public string? OutgoingTrustName { get; set; }
		public List<TransferringAcademyDto>? TransferringAcademies { get; set; }
        public AcademyTransferProjectFeaturesResponse? Features { get; set; }
        public AcademyTransferProjectDatesResponse? Dates { get; set; }
        public AcademyTransferProjectBenefitsResponse? Benefits { get; set; }
        public AcademyTransferProjectLegalRequirementsResponse? LegalRequirements { get; set; }
        public AcademyTransferProjectRationaleResponse? Rationale { get; set; }
        public AcademyTransferProjectGeneralInformationResponse? GeneralInformation { get; set; }
        public AssignedUserResponse? AssignedUser { get; set; }
        public string? State { get; set; }
        public string? Status { get; set; }
		public bool? IsFormAMat { get; set; }
		public DateTime? ProjectSentToCompleteDate { get; set; }
		
		public string? IncomingTrustReferenceNumber { get; set; }
		public AcademyTransferPublicSectorEqualityDutyResponse? PublicSectorEqualityDuty { get; set; }
	}
}
