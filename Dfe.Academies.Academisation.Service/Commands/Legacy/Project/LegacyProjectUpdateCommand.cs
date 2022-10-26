using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project;

public class LegacyProjectUpdateCommand : ILegacyProjectUpdateCommand
{
	private readonly IProjectGetDataQuery _projectGetDataQuery;
	private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

	public LegacyProjectUpdateCommand(IProjectGetDataQuery projectGetDataQuery, IProjectUpdateDataCommand projectUpdateDataCommand)
	{
		_projectGetDataQuery = projectGetDataQuery;
		_projectUpdateDataCommand = projectUpdateDataCommand;
	}

	public async Task<CommandResult> Execute(LegacyProjectServiceModel legacyProjectServiceModel)
	{
		var existingProject = await _projectGetDataQuery.Execute(legacyProjectServiceModel.Id);

		if (existingProject is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingProject.Update(LegacyProjectDetailsMapper.MapNonEmptyFields(legacyProjectServiceModel, existingProject));

		if (result is CommandValidationErrorResult)
		{
			return result;
		}
		if (result is not CommandSuccessResult)
		{
			throw new NotImplementedException();
		}

		await _projectUpdateDataCommand.Execute(existingProject);

		return result;
	}
}
