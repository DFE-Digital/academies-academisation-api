using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("project-group")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ProjectGroupController(IMediator mediator, ILogger<ProjectGroupController> logger, IProjectGroupQueryService projectGroupQueryService) : ControllerBase
	{

		[HttpPost("/project-group/create-project-group", Name = "CreateProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ProjectGroupResponseModel>> CreateProjectGroup(
			[FromBody] CreateProjectGroupCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Creating project group: {command}");
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CreateSuccessResult<ProjectGroupResponseModel> successResult => Ok(successResult.Payload),
				CreateValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpPut("{urn}/set-project-group", Name = "SetProjectGroup")]
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

		[HttpGet("{urn}/get-project-groups", Name = "GetProjectGroups")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<ProjectGroupResponseModel>>> GetProjectGroups(ProjectGroupSearchModel searchModel, CancellationToken cancellationToken)
		{
			var result = await projectGroupQueryService.GetProjectGroupsAsync(searchModel, cancellationToken);

			return result is null || result.Data.IsNullOrEmpty() ? NotFound() : Ok(result);
		}
	}
}
