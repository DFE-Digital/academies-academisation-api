using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class ApplicationSubmitCommand : IApplicationSubmitCommand
	{
		private readonly IApplicationGetDataQuery _dataQuery;
		private readonly IApplicationUpdateDataCommand _dataCommand;
		private readonly IProjectFactory _projectFactory;
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;

		public ApplicationSubmitCommand(IApplicationGetDataQuery dataQuery, IApplicationUpdateDataCommand dataCommand,
			IProjectFactory projectFactory, IProjectCreateDataCommand projectCreateDataCommand)
		{
			_dataQuery = dataQuery;
			_dataCommand = dataCommand;
			_projectFactory = projectFactory;
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<CommandResult> Execute(int applicationId)
		{
			var application = await _dataQuery.Execute(applicationId);

			if (application is null)
			{
				return new NotFoundCommandResult();
			}

			var domainResult = application.Submit();

			switch (domainResult)
			{
				case CommandValidationErrorResult:
					return domainResult;
				case CommandSuccessResult:
					break;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}

			if (application.ApplicationType == Domain.Core.ApplicationAggregate.ApplicationType.JoinAMat)
			{
				var projectResult = _projectFactory.Create(application, new EstablishmentDetails("not implemented"), new MisEstablishmentDetails(0));

				switch (projectResult)
				{
					case CreateValidationErrorResult<IProject> createValidationErrorResult:
						return new CommandValidationErrorResult(createValidationErrorResult.ValidationErrors);
					case CreateSuccessResult<IProject> createSuccessResult:
						await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
						break;
					default:
						throw new NotImplementedException("Other CreateResult types not expected");
				}				
			}

			await _dataCommand.Execute(application);
			return domainResult;
		}
	}
}
