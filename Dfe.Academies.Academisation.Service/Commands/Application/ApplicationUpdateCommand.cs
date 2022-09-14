﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

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
		if (existingApplication is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingApplication.Update(
			applicationServiceModel.ApplicationType,
			applicationServiceModel.ApplicationStatus,
			applicationServiceModel.Contributors.Select(c => new KeyValuePair<int, ContributorDetails>(c.ContributorId, c.ToDomain())),
			applicationServiceModel.Schools.Select(s => new KeyValuePair<int, SchoolDetails>(s.Id, s.ToDomain()))
			);

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
