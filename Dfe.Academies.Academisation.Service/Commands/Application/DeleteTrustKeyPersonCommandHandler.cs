using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
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

			var keyPersonToDelete = existingApplication.FormTrust!.KeyPeople.SingleOrDefault(x => x.Id == command.KeyPersonId);

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

			//TODO: This can be removed when there is no longer a disconnect between domain and persistence entities

			//if (keyPersonToDelete != null)
			//{
			//	foreach (var role in keyPersonToDelete.Roles)
			//	{
			//		await _applicationRepository.DeleteChildObjectById<TrustKeyPersonRoleState>(role.Id);
			//	}
			//}

			//await _applicationRepository.DeleteChildObjectById<TrustKeyPersonState>(command.KeyPersonId);

			return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(new CancellationToken())
				? new CommandSuccessResult()
				: new BadRequestCommandResult();
		}

	}
}
