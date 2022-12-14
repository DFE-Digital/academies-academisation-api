using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("legacy/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class LegacyProjectController : ControllerBase
	{
		private readonly ILegacyProjectAddNoteCommand _legacyProjectAddNoteCommand;
		private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
		private readonly ILegacyProjectListGetQuery _legacyProjectListGetQuery;
		private readonly ILegacyProjectUpdateCommand _legacyProjectUpdateCommand;
		private readonly IProjectGetStatusesQuery _projectGetStatusesQuery;

		public LegacyProjectController(ILegacyProjectGetQuery legacyProjectGetQuery,
									   ILegacyProjectListGetQuery legacyProjectListGetQuery,
									   IProjectGetStatusesQuery projectGetStatusesQuery,
									   ILegacyProjectUpdateCommand legacyProjectUpdateCommand,
									   ILegacyProjectAddNoteCommand legacyProjectAddNoteCommand)
		{
			_legacyProjectGetQuery = legacyProjectGetQuery;
			_legacyProjectListGetQuery = legacyProjectListGetQuery;
			_projectGetStatusesQuery = projectGetStatusesQuery;
			_legacyProjectUpdateCommand = legacyProjectUpdateCommand;
			_legacyProjectAddNoteCommand = legacyProjectAddNoteCommand;
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
		public async Task<ActionResult<LegacyApiResponse<LegacyProjectServiceModel>>> GetProjects(
			GetAcademyConversionSearchModel? searchModel,
			[FromQuery] int? urn = null)
		{
			LegacyApiResponse<LegacyProjectServiceModel>? result =
				await _legacyProjectListGetQuery.GetProjects(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count, urn,
					searchModel.RegionUrnsQueryString);
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
			ProjectFilterParameters result = await _projectGetStatusesQuery.Execute();
			return Ok(result);
		}

		/// <summary>
		/// Returns the project with the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <returns><see cref="LegacyProjectServiceModel"/></returns>
		/// <response code="200">The project with the specified ID was found and returned</response>
		/// <response code="404">The project with the specified ID was not found</response>
		[HttpGet("project/{id:int}", Name = "GetLegacyProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
		{
			LegacyProjectServiceModel? result = await _legacyProjectGetQuery.Execute(id);
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
		public async Task<ActionResult<LegacyProjectServiceModel>> Patch(
			int id,
			LegacyProjectServiceModel projectUpdate)
		{
			CommandResult result = await _legacyProjectUpdateCommand.Execute(id, projectUpdate);

			return result switch
			{
				CommandSuccessResult => Ok(await _legacyProjectGetQuery.Execute(id)),
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
			CommandResult result = await _legacyProjectAddNoteCommand.Execute(note.ToAddNoteModel(id));

			return result switch
			{
				CommandSuccessResult => Created(new Uri($"/legacy/project/{id}", UriKind.Relative), null),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}
	}
}
