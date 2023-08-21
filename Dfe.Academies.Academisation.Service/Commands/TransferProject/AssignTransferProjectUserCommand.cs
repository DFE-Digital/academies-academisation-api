using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class AssignTransferProjectUserCommand : SetTransferProjectCommand
	{
		public Guid UserId { get; set; }
		public string UserEmail{ get; set; }
		public string UserFullName { get; set; }
	}
}
