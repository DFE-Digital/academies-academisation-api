using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupAssignUserCommand(Guid userId, string fullName, string emailAddress) : IRequest<CommandResult>
	{
		public string GroupReferenceNumber { get; set; } = string.Empty;
		public Guid UserId { get; set; } = userId;
		public string FullName { get; set; } = fullName;
		public string EmailAddress { get; set; } = emailAddress;
	}
}
