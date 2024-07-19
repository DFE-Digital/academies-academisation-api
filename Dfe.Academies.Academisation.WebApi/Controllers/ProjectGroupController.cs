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
				null => BadRequest(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}
	}
}
