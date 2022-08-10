using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;
using System.Linq;

namespace Dfe.Academies.Academisation.Service.Commands;

public class ApplicationUpdateCommand : IApplicationUpdateCommand
{
	private readonly IApplicationGetDataQuery _applicationGetDataQuery;
	private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

	public ApplicationUpdateCommand(IApplicationGetDataQuery applicationGetDataQuery, IApplicationUpdateDataCommand applicationUpdateDataCommand)
	{
		_applicationGetDataQuery = applicationGetDataQuery;
		_applicationUpdateDataCommand = applicationUpdateDataCommand;
	}

	public async Task<CommandResult> Execute(int applicationId, ApplicationServiceModel applicationServiceModel)
	{
		if (applicationId != applicationServiceModel.ApplicationId)
		{
			return new CommandValidationErrorResult(
				new List<ValidationError>() {
					new ValidationError("Id", "Ids must be the same")
				});
		}

		var existingApplication = await _applicationGetDataQuery.Execute(applicationId);
		if (existingApplication is null) return new NotFoundCommandResult();

		var result = existingApplication.Update(
			applicationServiceModel.ApplicationType,
			applicationServiceModel.ApplicationStatus,
			applicationServiceModel.Contributors.ToDictionary(c => c.ContributorId, c => c.ToDomain()),
			applicationServiceModel.Schools.ToDictionary(s => s.Id, s => s.ToDomain())
			);

		if (result is CommandSuccessResult)
		{
			await _applicationUpdateDataCommand.Execute(existingApplication);
			return result;
		}
		if (result is CommandValidationErrorResult)
		{
			return result;
		}
		else
		{
			throw new NotImplementedException();
		}
	}
}
