using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate; 
using Dfe.Academies.Academisation.IService.ServiceModels.UserRole; 
using Dfe.Academies.Academisation.Service.Commands.UserRole;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("user-role")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class UserRoleController(IMediator mediator, ILogger<UserRoleController> logger, IUserRoleQueryService userRoleQueryService) : ControllerBase
	{
		[HttpPost("/user-role/create-user-role", Name = "CreateUserRole")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating user role: {value}", command);
			var result = await mediator.Send(command, cancellationToken);

			return result switch
			{
				CommandSuccessResult => Ok(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}
		[HttpPut("{email}/assign-user-role", Name = "SetUserRole")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SetUserRole(string email, [FromBody] SetUserRoleCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting user with a different role: {value}", command);
			command.EmailAddress = email;
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpGet("{email}", Name = "GetUserRoleCapabilities")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<RoleCapabilitiesModel>> GetUserRoleCapabilities(string email, CancellationToken cancellationToken)
		{
			var roleCapabiltiesModel = await userRoleQueryService.GetUserRoleCapabilitiesAsync(email, cancellationToken);

			if (roleCapabiltiesModel == null || roleCapabiltiesModel.Capabilities.Count == 0)
			{
				return NotFound($"User role capabilities with {email} email not found.");
			}

			return Ok(roleCapabiltiesModel);
		}

		[HttpPost("/user-role/search-users", Name = "SearchUsers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<UserRoleModel>>> SearchUsers(UserRoleSearchModel searchModel, CancellationToken cancellationToken)
		{
			var result = await userRoleQueryService.SearchUsersAsync(searchModel.searchTerm, searchModel.RoleId, searchModel.Page, searchModel.Count, cancellationToken);

			return result is null ? NotFound() : Ok(result);
		}
	}
}
