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

	public LegacyProjectController(ILegacyProjectGetQuery legacyProjectGetQuery)
	{
		_legacyProjectGetQuery = legacyProjectGetQuery;
	}

	[HttpGet("{id}", Name = "GetLegacyProject")]
	public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
	{
		var result = await _legacyProjectGetQuery.Execute(id);
		return result is null ? NotFound() : Ok(result);
	}
}
