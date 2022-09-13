using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Project;

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

		var result = existingProject.UpdatePatch(legacyProjectServiceModel.ToDomain());
				
		await _projectUpdateDataCommand.Execute(existingProject);

		return result;
	}
}
