using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.UserRoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.UserRole
{
	public class SetUserRoleCommandHandler(IUserRoleRepository userRoleRepository, IDateTimeProvider dateTimeProvider, ILogger<CreateUserRoleCommandHandler> logger, IRoleInfo roleInfo) : IRequestHandler<SetUserRoleCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetUserRoleCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating user role: {value}", message);

			var userRole = await userRoleRepository.GetUserWithRoleAsync(message.EmailAddress, cancellationToken);
			if(userRole == null)
			{
				return new NotFoundCommandResult();
			}
			userRole.SetRole(roleInfo.GetId(message.RoleId), dateTimeProvider.Now, message.IsEnabled);

			userRoleRepository.Update((Domain.UserRoleAggregate.UserRole)userRole);
			await userRoleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
