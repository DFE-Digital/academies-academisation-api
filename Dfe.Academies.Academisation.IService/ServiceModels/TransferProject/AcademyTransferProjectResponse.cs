
namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class AcademyTransferProjectResponse
    {
        public string ProjectUrn { get; set; }

        public string ProjectReference { get; set; }
        public string OutgoingTrustUkprn { get; set; }
        public List<TransferringAcademiesResponse> TransferringAcademies { get; set; }
        public AcademyTransferProjectFeaturesResponse Features { get; set; }
        public AcademyTransferProjectDatesResponse Dates { get; set; }
        public AcademyTransferProjectBenefitsResponse Benefits { get; set; }
        public AcademyTransferProjectLegalRequirementsResponse LegalRequirements { get; set; }
        public AcademyTransferProjectRationaleResponse Rationale { get; set; }
        public AcademyTransferProjectGeneralInformationResponse GeneralInformation { get; set; }
        public AssignedUserResponse AssignedUser { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
    }
}
