using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class ApplicationSubmitCommand : IApplicationSubmitCommand
	{
		private readonly IApplicationGetDataQuery _dataQuery;
		private readonly IApplicationUpdateDataCommand _dataCommand;
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		private readonly IApplicationSubmissionService _applicationSubmissionService;

		public ApplicationSubmitCommand(
			IApplicationGetDataQuery dataQuery,
			IApplicationUpdateDataCommand dataCommand,
			IProjectCreateDataCommand projectCreateDataCommand,
			IApplicationSubmissionService applicationSubmissionService)
		{
			_dataQuery = dataQuery;
			_dataCommand = dataCommand;
			_projectCreateDataCommand = projectCreateDataCommand;
			_applicationSubmissionService = applicationSubmissionService;
		}

		public async Task<CommandOrCreateResult> Execute(int applicationId)
		{
			var application = await _dataQuery.Execute(applicationId);

			if (application is null)
			{
				return new NotFoundCommandResult();
			}

			var domainServiceResult = _applicationSubmissionService.SubmitApplication(application);
			
			switch (domainServiceResult)
			{
				case CommandValidationErrorResult:
					return domainServiceResult;
				case CreateValidationErrorResult<IProject> createValidationErrorResult:
					return domainServiceResult;
				case CommandSuccessResult:
					break;
				case CreateSuccessResult<IProject> createSuccessResult:
					await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					break;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
			
			await _dataCommand.Execute(application);

			switch (domainServiceResult)
			{
				case CommandResult:
					return domainServiceResult;
				case CreateValidationErrorResult<IProject>:
					// ToDo map this type
					return domainServiceResult;
				case CreateSuccessResult<IProject> createSuccessResult:
					return createSuccessResult.MapToPayloadType(p => p.MapToServiceModel());
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
		}
	}
}
