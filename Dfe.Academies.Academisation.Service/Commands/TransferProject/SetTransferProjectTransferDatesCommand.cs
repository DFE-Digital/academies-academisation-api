using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommand : SetTransferProjectCommand
	{
		public DateTime? HtbDate { get; set; }
		public DateTime? PreviousAdvisoryBoardDate { get; set; }
		public DateTime? TargetDateForTransfer { get; set; }
		public bool? IsCompleted { get; set; }
		public string? ChangedBy { get; set; }
		public List<ReasonChange>? ReasonsChanged { get; set; }
	}
}
