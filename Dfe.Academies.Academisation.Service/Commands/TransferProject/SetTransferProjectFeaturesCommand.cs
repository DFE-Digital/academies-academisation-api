using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommand : SetTransferProjectCommand
	{	
		public string WhoInitiatedTheTransfer { get; set; }
		public List<string> SpecificReasonsForTransfer { get; set; }
		public string TypeOfTransfer { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
