using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectLegalRequirementsCommand : SetTransferProjectCommand
	{
		public string OutgoingTrustConsent { get; set; }
		public string IncomingTrustAgreement { get; set; }
		public string DiocesanConsent { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
