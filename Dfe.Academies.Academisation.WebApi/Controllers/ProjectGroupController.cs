﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup; 
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("project-group/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ProjectGroupController(IMediator mediator, ILogger<ProjectGroupController> logger, IProjectGroupQueryService projectGroupQueryService) : ControllerBase
	{

		[HttpPost("create-project-group", Name = "CreateProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ProjectGroupResponseModel>> CreateProjectGroup(
			[FromBody] CreateProjectGroupCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating project group: {value}", command);
			var result = await mediator.Send(command, cancellationToken);

			return result switch
			{
				CreateSuccessResult<ProjectGroupResponseModel> successResult => Ok(successResult.Payload),
				CreateValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpPut("{referenceNumber}/set-project-group", Name = "SetProjectGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SetProjectGroup(string referenceNumber, [FromBody] SetProjectGroupCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting project group: {value}", command);
			command.GroupReferenceNumber = referenceNumber;
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}


		[HttpPut("{referenceNumber}/assign-project-group-user", Name = "AssignProjectGroupUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AssignProjectGroupUser(string referenceNumber, [FromBody] SetProjectGroupAssignUserCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting project group with user: {value}", command);
			command.GroupReferenceNumber = referenceNumber;
			var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpPost("get-project-groups", Name = "GetProjectGroups")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<ProjectGroupResponseModel>>> GetProjectGroups(ConversionProjectSearchModel? searchModel, CancellationToken cancellationToken)
		{
			PagedDataResponse<ProjectGroupResponseModel>? result =
				await projectGroupQueryService.GetProjectGroupsAsync(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString,
					searchModel.RegionQueryString, searchModel.LocalAuthoritiesQueryString, searchModel.AdvisoryBoardDatesQueryString, 
					searchModel.Page, searchModel.Count, cancellationToken);

			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("{id:int}", Name = "GetProjectGrouptById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProjectGroupResponseModel>> GetProjectGrouptById(int id, CancellationToken cancellationToken)
		{
			var project = await projectGroupQueryService.GetProjectGroupByIdAsync(id, cancellationToken);

			if (project == null)
			{
				return NotFound($"Project group with ID {id} not found.");
			}

			return Ok(project);
		}

		[HttpDelete("{referenceNumber}", Name = "DeleteProjectGroupByReference")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProjectGroupResponseModel>> DeleteProjectGroupByReference(string referenceNumber, CancellationToken cancellationToken)
		{
			var result = await mediator.Send(new DeleteProjectGroupCommand(referenceNumber), cancellationToken);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}
	}
}
