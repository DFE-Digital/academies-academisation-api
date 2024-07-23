using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("project-group")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ProjectGroupController(IMediator mediator, ILogger<ProjectGroupController> logger, IProjectGroupQueryService projectGroupQueryService) : ControllerBase
	{

		[HttpPost(Name = "CreateProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> CreateProjectGroup(
			[FromBody] CreateProjectGroupCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Creating project group: {command}");
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				BadRequestCommandResult => BadRequest(new { Error = "One or more conversions already associated to another project group" }),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpPost("{urn}/set-project-group", Name = "SetProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SetProjectGroup(string urn, [FromBody] SetProjectGroupCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Setting project group: {command}");
			command.Urn = urn;
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpGet("{urn}/get-project-group", Name = "GetProjectGroupByUrn")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProjectGroupServiceModel>> GetProjectGroupByUrn(string urn, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Getting project group with urn: {urn}");
			var result = await projectGroupQueryService.GetProjectGroupByUrn(urn, cancellationToken);

			return result is null ? NotFound() : Ok(result);
		}
	}
}
