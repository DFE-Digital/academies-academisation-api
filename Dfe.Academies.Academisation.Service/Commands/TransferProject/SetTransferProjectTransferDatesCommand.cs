using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommand : SetTransferProjectCommand
	{
		public DateTime? HtbDate { get; set; }
		public DateTime? TargetDateForTransfer { get; set; }
	}
}
