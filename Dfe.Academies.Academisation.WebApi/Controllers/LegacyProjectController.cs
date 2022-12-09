using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
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
		/// <param name="urn">URN of a specific project to retrieve</param>
		/// <remarks>
		///     Filters are cumulative (AND logic), applied in the following order: by Region, by Status, by URN, by School, by
		///     Delivery Officer.
		/// </remarks>
		[HttpPost("projects", Name = "GetLegacyProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
		[HttpGet("projects/status")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProjectFilterParameters>> GetFilterParameters()
		{
			ProjectFilterParameters result = await _projectGetStatusesQuery.Execute();
			return Ok(result);
		}

		[HttpGet("project/{id}", Name = "GetLegacyProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LegacyProjectServiceModel>> Get(int id)
		{
			LegacyProjectServiceModel? result = await _legacyProjectGetQuery.Execute(id);
			return result is null ? NotFound() : Ok(result);
		}


		[HttpPatch("project/{id}", Name = "PatchLegacyProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LegacyProjectServiceModel>> Patch(
			int id,
			LegacyProjectServiceModel projectUpdate)
		{
			CommandResult result = await _legacyProjectUpdateCommand.Execute(projectUpdate with { Id = id });

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
		[HttpPost("project/{id}/notes")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> AddNote(int id, ProjectNote note)
		{
			CommandResult result = await _legacyProjectAddNoteCommand.Execute(
				new LegacyProjectAddNoteModel(id,
					note.Subject,
					note.Note,
					note.Author));

			return result switch
			{
				CommandSuccessResult => Created(new Uri($"/legacy/project/{id}", UriKind.Relative), null),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}
	}
}
