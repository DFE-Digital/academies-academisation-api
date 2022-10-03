using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class JoinTrustCommandHandler : ISetJoinTrustDetailsCommandHandler
	{

		private readonly IApplicationGetDataQuery _applicationGetDataQuery;
		private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

		public JoinTrustCommandHandler(IApplicationGetDataQuery applicationGetDataQuery, IApplicationUpdateDataCommand applicationUpdateDataCommand)
		{
			_applicationGetDataQuery = applicationGetDataQuery;
			_applicationUpdateDataCommand = applicationUpdateDataCommand;
		}

		public async Task<CommandResult> Handle(int applicationId, SetJoinTrustDetailsCommand command)
		{
			var existingApplication = await _applicationGetDataQuery.Execute(applicationId);
			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.SetJoinTrustDetails(command.UkPrn, command.trustName);

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
