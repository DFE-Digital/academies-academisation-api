using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
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
	private readonly IProjectGetStatusesQuery _projectGetStatusesQuery;
	private readonly ILegacyProjectUpdateCommand _legacyProjectUpdateCommand;

	public LegacyProjectController(ILegacyProjectGetQuery legacyProjectGetQuery, ILegacyProjectListGetQuery legacyProjectListGetQuery,
		IProjectGetStatusesQuery projectGetStatusesQuery, ILegacyProjectUpdateCommand legacyProjectUpdateCommand)
	{
		_legacyProjectGetQuery = legacyProjectGetQuery;
		_legacyProjectListGetQuery = legacyProjectListGetQuery;
		_projectGetStatusesQuery = projectGetStatusesQuery;
		_legacyProjectUpdateCommand = legacyProjectUpdateCommand;
	}

	[HttpGet("projects", Name = "GetLegacyProjects")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<LegacyApiResponse<LegacyProjectServiceModel>>> GetProjects(
		[FromQuery] string? states,
		[FromQuery] string? title,
		[FromQuery] string[]? deliveryOfficers,
		[FromQuery] int page = 1,		
		[FromQuery] int count = 50,
		[FromQuery] int? urn = null,
		[FromQuery] int[]? regions = default)
	{
		var result = await _legacyProjectListGetQuery.GetProjects(states, title, deliveryOfficers, page, count, urn, regions);
		return result is null ? NotFound() : Ok(result);
	}

	[HttpGet("projects/status")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<List<string>>> GetStatuses()
	{
		var result = await _projectGetStatusesQuery.Execute();
		return Ok(result);
	}

	[HttpGet("project/{id}", Name = "GetLegacyProject")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
	{
		var result = await _legacyProjectGetQuery.Execute(id);
		return result is null ? NotFound() : Ok(result);
	}


	[HttpPatch("project/{id}", Name = "PatchLegacyProject")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<LegacyProjectServiceModel>> Patch(int id, LegacyProjectServiceModel projectUpdate)
	{
		var result = await _legacyProjectUpdateCommand.Execute(projectUpdate with { Id = id });
		
		return result switch
		{
			CommandSuccessResult => Ok(await _legacyProjectGetQuery.Execute(id)),
			NotFoundCommandResult => NotFound(),
			CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException()
		};
	}
}
