using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademyGeneralInformationCommand : SetTransferProjectCommand
	{
		public string TransferringAcademyUkprn { get; set; }
		public string PFIScheme { get; set; }
		public string PFISchemeDetails { get; set; }
	}
}

