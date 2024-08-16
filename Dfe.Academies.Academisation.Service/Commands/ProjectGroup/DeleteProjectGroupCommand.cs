using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class DeleteProjectGroupCommand(string referenceNumber) : IRequest<CommandResult>
	{
		public string GroupReferenceNumber { get; set; } = referenceNumber;
	}
}
