using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("project-group")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ProjectGroupController : ControllerBase
	{
		private readonly ILogger<ProjectGroupController> _logger;
		private readonly IMediator _mediator;

		public ProjectGroupController(IMediator mediator, ILogger<ProjectGroupController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost(Name = "CreateProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> CreateProjectGroup(
			[FromBody] CreateProjectGroupCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Creating project group: {command}");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

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
			_logger.LogInformation($"Setting project group: {command}");
			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpGet("{urn}/get-project-group", Name = "GetProjectGroupById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProjectGroupDto>> GetProjectGroupById(string urn, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Getting project group with urn: {urn}");
			var query = new GetProjectGroupQueryCommand(urn);
			var result = await _mediator.Send(query, cancellationToken);

			if (result is null)
			{
				return NotFound();
			}

			return result is null? NotFound() : Ok(result);
		}
	}
}
