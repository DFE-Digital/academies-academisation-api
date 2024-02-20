using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("legacy/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ProjectController : ControllerBase
	{
		private readonly ICreateNewProjectCommand _createNewProjectCommand;

		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly IMediator _mediator;

		public ProjectController(

									   ICreateNewProjectCommand createSponsoredProjectCommand,

									   IConversionProjectQueryService conversionProjectQueryService,
									   IMediator mediator)
		{
			_createNewProjectCommand = createSponsoredProjectCommand;

			_conversionProjectQueryService = conversionProjectQueryService;
			_mediator = mediator;
		}

		/// <summary>
		///     Retrieve all projects matching specified filter conditions
		/// </summary>
		/// <param name="searchModel"><see cref="GetAcademyConversionSearchModel"/> describing filtering requirements for the request</param>
		/// <param name="urn">URN of a specific project to retrieve</param>
		/// <remarks>
		///     Filters are cumulative (AND logic), applied in the following order: by Region, by Status, by URN, by School, by
		///     Delivery Officer.
		/// </remarks>
		/// <response code="200">One or more projects matching the specified filter criteria were found</response>
		/// <response code="404">No projects matched the specified search criteria</response>
		[HttpPost("projects", Name = "GetLegacyProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<ConversionProjectServiceModel>>> GetProjects(
			GetAcademyConversionSearchModel? searchModel,
			[FromQuery] int? urn = null)
		{
			PagedDataResponse<ConversionProjectServiceModel>? result =
				await _conversionProjectQueryService.GetProjects(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count, urn,
					searchModel.RegionQueryString, searchModel.ApplicationReferences);
			return result is null ? NotFound() : Ok(result);
		}

		/// <summary>
		///     Returns a list of statuses and assigned users available to be used in filters
		/// </summary>
		/// <remarks>
		///     Statuses or users not in use in the current project data will not be present in the returned data
		/// </remarks>
		/// <response code="200">details of available filter parameters are provided</response>
		[HttpGet("projects/status")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ProjectFilterParameters>> GetFilterParameters()
		{
			ProjectFilterParameters result = await _conversionProjectQueryService.GetFilterParameters();
			return Ok(result);
		}

		/// <summary>
		/// Returns the project with the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <returns><see cref="ConversionProjectServiceModel"/></returns>
		/// <response code="200">The project with the specified ID was found and returned</response>
		/// <response code="404">The project with the specified ID was not found</response>
		[HttpGet("project/{id:int}", Name = "GetLegacyProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ConversionProjectServiceModel>> Get(int id)
		{
			ConversionProjectServiceModel? result = await _conversionProjectQueryService.GetConversionProject(id);
			return result is null ? NotFound() : Ok(result);
		}


		/// <summary>
		/// Updates the project with the specified id using the provided partial data in <paramref name="projectUpdate"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="projectUpdate">The partial data describing the changes</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully and the updated project is returned</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPatch("project/{id:int}", Name = "PatchLegacyProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ConversionProjectServiceModel>> Patch(
			int id,
			ConversionProjectServiceModel projectUpdate)
		{
			CommandResult result = await _mediator.Send(new ConversionProjectUpdateCommand(id, projectUpdate));

			return result switch
			{
				CommandSuccessResult => Ok(await _conversionProjectQueryService.GetConversionProject(id)),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		/// <summary>
		///     Adds a note to the project with the specified ID
		/// </summary>
		/// <param name="id">The ID for the project to which the note should be added</param>
		/// <param name="note">Project Note data</param>
		/// <response code="404">The ID does not correspond to a known Project</response>
		/// <response code="201">The note has been added to the specified Project</response>
		[HttpPost("project/{id:int}/notes")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> AddNote(int id, AddNoteRequest note)
		{
			CommandResult result = await _mediator.Send(note.ToAddNoteModel(id));
			return result switch
			{
				CommandSuccessResult => Created(new Uri($"/legacy/project/{id}", UriKind.Relative), null),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}

		/// <summary>
		///     Adds a new conversion project
		/// </summary>
		/// <param name="project">The model holding the data required to create a new conversion</param>
		/// <response code="201">The project has been added</response>
		[HttpPost("project/new-conversion-project")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> AddConversion(NewProjectServiceModel project)
		{
			CommandResult result = await _createNewProjectCommand.Execute(project);

			return result switch
			{
				CommandSuccessResult => Created(new Uri("/legacy/project/", UriKind.Relative), null),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}

		/// <summary>
		///     Deletes the provided note from the project with the specified ID
		/// </summary>
		/// <param name="id">The ID for the project from which the note should be deleted</param>
		/// <param name="command">Project note data</param>
		/// <response code="404">
		///     Either the project with the specified ID is not found, or the project does not contain the
		///     provided note
		/// </response>
		/// <response code="204">The note has been deleted from the project</response>
		/// <exception cref="NotImplementedException">Thrown if the underlying command returns an unexpected result type</exception>
		[HttpDelete("project/{id:int}/notes")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteNote(int id, ConversionProjectDeleteNoteCommand command)
		{
			command.ProjectId = id;

			CommandResult result = await _mediator.Send(command);
			return result switch
			{
				CommandSuccessResult => NoContent(),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}
	}
}
