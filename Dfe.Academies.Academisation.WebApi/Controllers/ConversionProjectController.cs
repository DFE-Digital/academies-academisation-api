﻿namespace Dfe.Academies.Academisation.WebApi.Controllers
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
        [HttpPost("projects", Name = "GetProjects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LegacyApiResponse<ConversionProjectServiceModel>>> GetProjects(
                ConversionProjectSearchModel? searchModel,
                [FromQuery] int? urn = null)
        {
            LegacyApiResponse<ConversionProjectServiceModel>? result =
                await _conversionProjectQueryService.GetProjectsV2(searchModel!.StatusQueryString, searchModel.TitleFilter,
                    searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count,
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
    }
}