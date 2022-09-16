using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("legacy/")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class LegacyProjectController : ControllerBase
{
	private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
	private readonly ILegacyProjectListGetQuery _legacyProjectListGetQuery;
	private readonly ILegacyProjectUpdateCommand _legacyProjectUpdateCommand;

	public LegacyProjectController(ILegacyProjectGetQuery legacyProjectGetQuery, ILegacyProjectListGetQuery legacyProjectListGetQuery, 
		ILegacyProjectUpdateCommand legacyProjectUpdateCommand)
	{
		_legacyProjectGetQuery = legacyProjectGetQuery;
		_legacyProjectListGetQuery = legacyProjectListGetQuery;
		_legacyProjectUpdateCommand = legacyProjectUpdateCommand;
	}

	[HttpGet("projects", Name = "GetLegacyProjects")]
	public async Task<ActionResult<LegacyApiResponse<LegacyProjectServiceModel>>>GetProjects([FromQuery] string? states,
		[FromQuery] int page = 1,
		[FromQuery] int count = 50,
		[FromQuery] int? urn = null)
	{
		var result = await _legacyProjectListGetQuery.GetProjects(states, page, count, urn);
		return result is null ? NotFound() : Ok(result);
	}
	
	[HttpGet("project/{id}", Name = "GetLegacyProject")]
	public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
	{
		var result = await _legacyProjectGetQuery.Execute(id);
		return result is null ? NotFound() : Ok(result);
	}


	[HttpPatch("project", Name = "PatchLegacyProject")]
	public async Task<ActionResult<LegacyProjectServiceModel>> Patch(LegacyProjectServiceModel projectUpdate)
	{
		var result = await _legacyProjectUpdateCommand.Execute(projectUpdate);				

		return result switch
		{
			CommandSuccessResult => Ok(await _legacyProjectGetQuery.Execute(projectUpdate.Id)),
			NotFoundCommandResult => NotFound(),
			CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException()
		};		
	}
}
