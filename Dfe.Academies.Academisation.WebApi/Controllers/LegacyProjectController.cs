﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("legacy/project")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class LegacyProjectController : ControllerBase
{
	private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
	private readonly ILegacyProjectUpdateCommand _legacyProjectUpdateCommand;

	public LegacyProjectController(ILegacyProjectGetQuery legacyProjectGetQuery, ILegacyProjectUpdateCommand legacyProjectUpdateCommand)
	{
		_legacyProjectGetQuery = legacyProjectGetQuery;
		_legacyProjectUpdateCommand = legacyProjectUpdateCommand;
	}

	[HttpGet("{id}", Name = "GetLegacyProject")]
	public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
	{
		var result = await _legacyProjectGetQuery.Execute(id);
		return result is null ? NotFound() : Ok(result);
	}


	[HttpPatch(Name = "PatchLegacyProject")]
	public async Task<ActionResult<LegacyProjectServiceModel>> Patch(LegacyProjectServiceModel projectUpdate)
	{
		var result = await _legacyProjectUpdateCommand.Execute(projectUpdate);		

		return result switch
		{
			CommandSuccessResult => Ok(),
			NotFoundCommandResult => NotFound(),
			CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException()
		};		
	}
}
