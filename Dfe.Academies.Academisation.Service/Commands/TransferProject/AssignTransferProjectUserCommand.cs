using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class AssignTransferProjectUserCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public string UserEmail{ get; set; }
		public string UserFullName { get; set; }
	}
}
