using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademySchoolAdditionalDataCommand : SetTransferProjectCommand
	{
		public string TransferringAcademyUkprn { get; set; }
		public string LatestOfstedReportAdditionalInformation { get; set; }
		public string PupilNumbersAdditionalInformation { get; set; }
		public string KeyStage2PerformanceAdditionalInformation { get; set; }
		public string KeyStage4PerformanceAdditionalInformation { get; set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }
	}
}

