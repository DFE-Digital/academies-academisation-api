using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using Dfe.Academies.Academisation.Service.Commands.FormAMat;
using Dfe.Academies.Academisation.WebApi.Extensions;
using Dfe.Academies.Academisation.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("conversion-project/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ConversionProjectController : ControllerBase
	{
		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly IMediator _mediator;

		public ConversionProjectController(IConversionProjectQueryService conversionProjectQueryService,
									   IMediator mediator)
		{

			_conversionProjectQueryService = conversionProjectQueryService;
			_mediator = mediator;
		}

		/// <summary>
		/// Updates the project with the specified id sets external application form data using the data in the command <paramref name="request"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="request">the command containing the payload of updates</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPut("{id:int}/SetExternalApplicationForm", Name = "SetExternalApplicationForm")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetExternalApplicationForm(
			int id,
			SetExternalApplicationFormCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{id:int}/SetFormAMatAssignedUser", Name = "SetFormAMatAssignedUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetFormAMatAssignedUser(
		int id,
		[FromBody] SetFormAMatAssignedUserCommand request)
		{
			// Ensure the command's ID matches the route parameter
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{id:int}/SetAssignedUser", Name = "SetAssignedUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetAssignedUser(
										int id,
										[FromBody] SetAssignedUserCommand request)
		{
			// Ensure the command's ID matches the route parameter
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{id:int}/SetPerformancedata", Name = "SetPerformancedata")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetPerformanceData(
	int id,
	SetPerformanceDataCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetIncomingTrust", Name = "SetIncomingTrust")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetIncomingTrust(int id, SetIncomingTrustCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		/// <summary>
		///     Retrieve all projects matching specified filter conditions
		/// </summary>
		/// <param name="searchModel"><see cref="GetProjectSearchModel"/> describing filtering requirements for the request</param>
		/// <param name="urn">URN of a specific project to retrieve</param>
		/// <remarks>
		///     Filters are cumulative (AND logic), applied in the following order: by Region, by Status, by URN, by School, by
		///     Delivery Officer.
		/// </remarks>
		/// <response code="200">One or more projects matching the specified filter criteria were found</response>
		/// <response code="404">No projects matched the specified search criteria</response>
		[HttpPost("projects", Name = "GetProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<ConversionProjectServiceModel>>> GetProjects(
				ConversionProjectSearchModel? searchModel,
				[FromQuery] int? urn = null)
		{
			PagedDataResponse<ConversionProjectServiceModel>? result =
				await _conversionProjectQueryService.GetProjectsV2(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count,
					searchModel.RegionQueryString, searchModel.LocalAuthoritiesQueryString, searchModel.AdvisoryBoardDatesQueryString);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("projects-for-group/{trustReferenceNumber}", Name = "GetProjectsForGroup")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<ConversionProjectServiceModel>>> GetProjectsForGroup(
		string trustReferenceNumber, CancellationToken cancellationToken)
		{
			List<ConversionProjectServiceModel> result =
				await _conversionProjectQueryService.GetProjectsForGroup(trustReferenceNumber, cancellationToken);
			return result is null ? NotFound() : Ok(result);
		}

		/// <summary>
		///     Retrieve all form a mat projects matching specified filter conditions
		/// </summary>
		/// <param name="searchModel"><see cref="GetProjectSearchModel"/> describing filtering requirements for the request</param>
		/// <param name="urn">URN of a specific project to retrieve</param>
		/// <remarks>
		///     Filters are cumulative (AND logic), applied in the following order: by Region, by Status, by URN, by School, by
		///     Delivery Officer.
		/// </remarks>
		/// <response code="200">One or more projects matching the specified filter criteria were found</response>
		/// <response code="404">No projects matched the specified search criteria</response>
		[HttpPost("FormAMatProjects", Name = "GetFormAMatProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<FormAMatProjectServiceModel>>> GetFormAMatProjects(
				ConversionProjectSearchModel? searchModel, CancellationToken cancellationToken)
		{
			PagedDataResponse<FormAMatProjectServiceModel>? result =
				await _conversionProjectQueryService.GetFormAMatProjects(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count, cancellationToken,
					searchModel.RegionQueryString, searchModel.LocalAuthoritiesQueryString, searchModel.AdvisoryBoardDatesQueryString);
			return result is null ? NotFound() : Ok(result);
		}

		/// <summary>
		/// Updates the project with the specified id - Sets the School Overview using data from the command <paramref name="request"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="request">the command containing the payload of updates</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPut("{id:int}/SetSchoolOverview", Name = "SetSchoolOverview")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSchoolOverview(
			int id,
			SetSchoolOverviewCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("formamatproject/{id:int}", Name = "GetFormAMatProjectById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<FormAMatProjectServiceModel>> GetFormAMatProjectById(int id, CancellationToken cancellationToken)
		{
			var project = await _conversionProjectQueryService.GetFormAMatProjectById(id, cancellationToken);

			if (project == null)
			{
				return NotFound($"Project with ID {id} not found.");
			}

			return Ok(project);
		}
		/// <summary>
		/// Creates a new FormAMat project along with a child conversion project.
		/// </summary>
		/// <param name="command">The command containing the data needed to create the project</param>
		/// <returns>An ActionResult indicating the outcome of the operation</returns>
		[HttpPost("FormAMatProject", Name = "CreateFormAMatAndChildConversion")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> CreateFormAMatAndChildConversion(CreateFormAMatAndChildConversionCommand command)
		{
			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException("The command result is not recognized.")
			};
		}
		[HttpGet("search-formamatprojects", Name = "SearchFormAMatProjects")]
		[ProducesResponseType(typeof(IEnumerable<FormAMatProjectServiceModel>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<FormAMatProjectServiceModel>>> SearchFormAMatProjects([FromQuery] string searchTerm, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return BadRequest("Search term must not be empty.");
			}

			var projects = await _conversionProjectQueryService.SearchFormAMatProjectsByTermAsync(searchTerm, cancellationToken);

			if (projects == null || !projects.Any())
			{
				return NotFound($"No Form A Mat projects found matching search term '{searchTerm}'.");
			}

			return Ok(projects);
		}
		/// <summary>
		/// Updates the project with the specified id - Sets the Form A Mat Project Reference using data from the command <paramref name="request"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="request">the command containing the payload of updates</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPut("{id:int}/SetFormAMatProjectReference", Name = "SetFormAMatProjectReference")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetFormAMatProjectReference(
			int id,
			SetFormAMatProjectReferenceCommand request)
		{
			request.ProjectId = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		/// <summary>
		///     Adds a school improvement plan to the project with the specified ID
		/// </summary>
		/// <param name="id">The ID for the project to which the school improvement plan should be added</param>
		/// <param name="command">Add school improvement plan data</param>
		/// <response code="404">The ID does not correspond to a known Project</response>
		/// <response code="201">The school improvement plan has been added to the specified Project</response>
		[HttpPost("{id:int}/school-improvement-plans")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> AddSchooImprovementPlan(int id, ConversionProjectAddSchoolImprovementPlanCommand command, CancellationToken cancellationToken)
		{
			CommandResult result = await _mediator.Send(command, cancellationToken);
			return result switch
			{
				CommandSuccessResult => Created(new Uri($"/legacy/project/{id}", UriKind.Relative), null),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}


		[HttpGet("{id:int}/school-improvement-plans")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<SchoolImprovementPlanServiceModel>>> GetSchooImprovementPlans(int id, CancellationToken cancellationToken)
		{
			var project = await _conversionProjectQueryService.GetConversionProject(id, cancellationToken);
			if (project == null)

			{
				return NotFound($"Project with ID {id} not found.");
			}

			var schoolImprovementPlans = await _conversionProjectQueryService.GetSchoolImprovementPlansByConversionProjectId(id, cancellationToken);

			return Ok(schoolImprovementPlans);
		}

		[HttpPut("{id:int}/school-improvement-plans")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> UpdateSchoolImprovementPlan(
	int id,
	ConversionProjectUpdateSchoolImprovementPlanCommand request)
		{
			if (request.ProjectId != id)
			{
				return BadRequest();
			}

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpDelete("{id:int}/Delete", Name = "DeleteConversionProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteAProjectById(int id, CancellationToken cancellationToken)
		{
			SetDeletedAtCommand request = new SetDeletedAtCommand(id);

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}


		/// <summary>
		/// Updates the project with the specified id - Sets the Project Dates using data from the command <paramref name="request"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="request">the command containing the payload of updates</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPut("{id:int}/SetProjectDates", Name = "SetProjectDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetProjectDates(
			int id,
			SetProjectDatesCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{id}/conversion-date-history", Name = "GetOpeningDateHistoryForConversionProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<OpeningDateHistoryDto>>> GetOpeningDateHistoryForConversionProject(int id, CancellationToken cancellationToken)
		{
			var query = new GetOpeningDateHistoryQuery(nameof(Project), id);
			var result = await _mediator.Send(query, cancellationToken);

			if (result is null || !result.Any())
			{
				return NotFound();
			}

			return Ok(result);
		}

	}
}
