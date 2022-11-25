using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class UpdateTrustKeyPersonCommandHandler : IRequestHandler<UpdateTrustKeyPersonCommand, CommandResult>
	{

		private readonly IApplicationGetDataQuery _applicationGetDataQuery;
		private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

		public UpdateTrustKeyPersonCommandHandler(IApplicationGetDataQuery applicationGetDataQuery, IApplicationUpdateDataCommand applicationUpdateDataCommand)
		{
			_applicationGetDataQuery = applicationGetDataQuery;
			_applicationUpdateDataCommand = applicationUpdateDataCommand;
		}

		public async Task<CommandResult> Handle(UpdateTrustKeyPersonCommand command, CancellationToken cancellationToken)
		{
			// cancellation token will be passed down to the database requests when the repository pattern is brought in
			// shouldn't be anything that is long running just found it a good habit with async

			var existingApplication = await _applicationGetDataQuery.Execute(command.ApplicationId);
			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.UpdateTrustKeyPerson(command.KeyPersonId, command.FirstName, command.Surname, command.DateOfBirth, command.ContactEmailAddress, command.Role, command.TimeInRole, command.Biography);
			
			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}

			await _applicationUpdateDataCommand.Execute(existingApplication);

			return result;
		}

	}
}
