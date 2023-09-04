using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Microsoft.IdentityModel.Tokens;

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

	public async Task<CommandResult> Execute(int id, LegacyProjectServiceModel legacyProjectServiceModel)
	{
		var existingProject = await _projectGetDataQuery.Execute(id);

		if (existingProject is null)
		{
			return new NotFoundCommandResult();
		}
		
		// Sponsored Grant defaults
		string? existingConversionSupportGrantType = existingProject.Details?.ConversionSupportGrantType ?? null;
		string? newConversionSupportGrantType = legacyProjectServiceModel.ConversionSupportGrantType;
		decimal? grantAmount = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingConversionSupportGrantType, newConversionSupportGrantType, legacyProjectServiceModel.ConversionSupportGrantAmount);

		var result = existingProject.Update(legacyProjectServiceModel.MapNonEmptyFields(existingProject, grantAmount));

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
