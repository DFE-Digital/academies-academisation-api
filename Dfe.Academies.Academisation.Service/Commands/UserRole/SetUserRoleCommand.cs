using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.UserRole
{
	public class SetUserRoleCommand(RoleId roleId, bool isEnabled) : IRequest<CommandResult>
	{
		public RoleId RoleId { get; set; } = roleId;
		public bool IsEnabled { get; set; } = isEnabled;
		public string EmailAddress { get; set; } = string.Empty;
	}
}
