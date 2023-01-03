using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using MediatR;
using TrustKeyPerson = Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts.TrustKeyPerson;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class DeleteTrustKeyPersonCommandHandler : IRequestHandler<DeleteTrustKeyPersonCommand, CommandResult>
	{
		private readonly IApplicationRepository _applicationRepository;

		public DeleteTrustKeyPersonCommandHandler(IApplicationRepository applicationRepository)
		{
			_applicationRepository = applicationRepository;
		}

		public async Task<CommandResult> Handle(DeleteTrustKeyPersonCommand command, CancellationToken cancellationToken)
		{
			// cancellation token will be passed down to the database requests when the repository pattern is brought in
			// shouldn't be anything that is long running just found it a good habit with async

			var existingApplication = await _applicationRepository.GetByIdAsync(command.ApplicationId);

			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.DeleteTrustKeyPerson(command.KeyPersonId);
			
			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}

			_applicationRepository.Update(existingApplication);

			return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
				? new CommandSuccessResult()
				: new BadRequestCommandResult();
		}

	}
}
