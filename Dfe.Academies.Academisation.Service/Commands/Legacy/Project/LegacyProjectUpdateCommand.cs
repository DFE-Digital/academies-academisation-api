using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project;

public class LegacyProjectUpdateCommand : ILegacyProjectUpdateCommand
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

	public LegacyProjectUpdateCommand(IConversionProjectRepository conversionProjectRepository, IProjectUpdateDataCommand projectUpdateDataCommand)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_projectUpdateDataCommand = projectUpdateDataCommand;
	}

	public async Task<CommandResult> Execute(int id, LegacyProjectServiceModel legacyProjectServiceModel)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(id);

		if (existingProject is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingProject.Update(legacyProjectServiceModel.MapNonEmptyFields(existingProject));

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
