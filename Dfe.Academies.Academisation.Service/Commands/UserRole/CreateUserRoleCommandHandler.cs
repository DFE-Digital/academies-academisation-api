using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.UserRoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.UserRole
{
	public class CreateUserRoleCommandHandler(IUserRoleRepository userRoleRepository, IDateTimeProvider dateTimeProvider, IRoleInfo roleInfo, ILogger<CreateUserRoleCommandHandler> logger) : IRequestHandler<CreateUserRoleCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(CreateUserRoleCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating user role: {value}", message);

			var userRole = new Domain.UserRoleAggregate.UserRole(roleInfo.GetId(message.RoleId), message.IsEnabled, dateTimeProvider.Now);
			userRole.SetAssignedUser(message.UserId, message.FullName, message.EmailAddress);

			userRoleRepository.Insert(userRole);
			await userRoleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
