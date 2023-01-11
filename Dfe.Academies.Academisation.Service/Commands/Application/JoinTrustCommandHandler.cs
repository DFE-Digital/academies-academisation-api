using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class JoinTrustCommandHandler : IRequestHandler<SetJoinTrustDetailsCommand, CommandResult>
	{

		private readonly IApplicationRepository _applicationRepository;

		public JoinTrustCommandHandler(IApplicationRepository applicationRepository)
		{
			_applicationRepository = applicationRepository;
		}

		public async Task<CommandResult> Handle(SetJoinTrustDetailsCommand command, CancellationToken cancellationToken)
		{
			var existingApplication = await _applicationRepository.GetByIdAsync(command.applicationId);

			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.SetJoinTrustDetails(
				command.UKPRN,
				command.trustName,
				command.changesToTrust,
				command.changesToTrustExplained,
				command.changesToLaGovernance,
				command.changesToLaGovernanceExplained);

			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}
			_applicationRepository.Update(existingApplication);
			await _applicationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			return result;
		}
	}
}
