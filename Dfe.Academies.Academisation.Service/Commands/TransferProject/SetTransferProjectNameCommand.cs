using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectNameCommand : SetTransferProjectCommand
	{
		public string ProjectName { get; set; }
	}
}
