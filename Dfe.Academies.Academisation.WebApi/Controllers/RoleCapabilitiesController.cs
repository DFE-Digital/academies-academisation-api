﻿using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.RoleCapabilities;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("role-capabilities/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class RoleCapabilitiesController(ILogger<RoleCapabilitiesController> logger, IRoleCapabilitiesQueryService userRoleQueryService) : ControllerBase
	{
		[HttpPost("capabilities", Name = "GetRolesCapabilities")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<RoleCapabilitiesModel> GetRolesCapabilities(List<string> roles, CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting roles capablities with roles: {roles}", roles);
			var roleCapabiltiesModel = userRoleQueryService.GetRolesCapabilitiesAsync(roles);

			return Ok(roleCapabiltiesModel);
		}
	}
}
