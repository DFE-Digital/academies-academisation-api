using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommand : SetTransferProjectCommand
	{
		public int Id { get; set; }
		public string TypeOfTransfer { get; set; }
		public string WhoInitiatedTheTransfer { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
