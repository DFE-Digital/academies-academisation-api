using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademyGeneralInformationCommand : SetTransferProjectCommand
	{
		public string TransferringAcademyUkprn { get; set; }
		public string PFIScheme { get; set; }
		public string PFISchemeDetails { get; set; }
		public string DistanceFromAcademyToTrustHq { get; set; }
		public string DistanceFromAcademyToTrustHqDetails { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public string MPNameAndParty { get; set; }
		public string PublishedAdmissionNumber { get; set; }
	}
}

