using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademySchoolAdditionalDataCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
		public int TransferringAcademyId { get; set; }
		public string LatestOfstedReportAdditionalInformation { get; set; }
		public string PupilNumbersAdditionalInformation { get; set; }
		public string KeyStage2PerformanceAdditionalInformation { get; set; }
		public string KeyStage4PerformanceAdditionalInformation { get; set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }
	}
}
