using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTrustCommand : SetTransferProjectCommand
	{
		public string ProjectName { get; set; }
		public string? IncomingTrustUKPRN { get; set; }
	}
}
