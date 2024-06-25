using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("transfer-project")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class TransferProjectController : ControllerBase
	{
		private const string GET_BY_URN_ROUTE_NAME = "GetByUrn";
		private readonly IMediator _mediator;
		private readonly ILogger<TransferProjectController> _logger;
		private readonly ITransferProjectQueryService _transferProjectQueryService;


		public TransferProjectController(IMediator mediator, ITransferProjectQueryService transferProjectQueryService,
			ILogger<TransferProjectController> logger)
		{
			_mediator = mediator;
			_transferProjectQueryService = transferProjectQueryService;
			_logger = logger;
		}


		[HttpPost(Name = "CreateTransferProject")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AcademyTransferProjectResponse>> CreateTransferProject(
			[FromBody] CreateTransferProjectCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Creating transfer project");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CreateSuccessResult<AcademyTransferProjectResponse> successResult => CreatedAtRoute(GET_BY_URN_ROUTE_NAME, new { urn = successResult.Payload.ProjectUrn }, successResult.Payload),
				null => BadRequest(),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-rationale", Name = "SetTransferProjectRationale")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectRationale(int urn,
			[FromBody] SetTransferProjectRationaleCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project rationale");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{urn}/set-transfer-dates", Name = "SetTransferProjectTransferDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectTransferDates(int urn,
			[FromBody] SetTransferProjectTransferDatesCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project transfer dates");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-legal-requirements", Name = "SetTransferProjectLegalRequirements")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectLegalRequirements(int urn,
			[FromBody] SetTransferProjectLegalRequirementsCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project legal requirements");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-status", Name = "SetTransferProjectStatus")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectStatus(int urn,
			[FromBody] SetTransferProjectStatusCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project status");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-trust", Name = "SetTransferProjectTrust")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectTrust(int urn,
			[FromBody] SetTransferProjectTrustCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project trust");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-features", Name = "SetTransferProjectFeatures")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectFeatures(int urn,
			[FromBody] SetTransferProjectFeaturesCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project features");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-benefits", Name = "SetTransferProjectBenefits")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectBenefits(int urn,
		[FromBody] SetTransferProjectBenefitsCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project benefits");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{urn}/set-school-additional-data", Name = "SetSchoolAdditionalData")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetSchoolAdditionalData(int urn,
			[FromBody] SetTransferringAcademySchoolAdditionalDataCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transferring academy school additional data");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{urn}/set-academy-general-information", Name = "SetTransferringAcademyGeneralInformation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferringAcademyGeneralInformation(int urn,
	[FromBody] SetTransferringAcademyGeneralInformationCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transferring academy general information");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{urn}/set-general-information", Name = "SetTransferProjectGeneralInformation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectGeneralInformation(int urn,
			[FromBody] SetTransferProjectGeneralInformationCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project trust information and project dates");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{urn}/assign-user", Name = "AssignUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AssignUser(int urn,
			[FromBody] AssignTransferProjectUserCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Assigning user to transfer project");

			command.Urn = urn;
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{urn}", Name = GET_BY_URN_ROUTE_NAME)]
		public async Task<ActionResult<AcademyTransferProjectResponse>> GetByUrn(int urn)
		{
			_logger.LogInformation($"Getting transfer project, urn: {urn}");

			AcademyTransferProjectResponse? result = await _transferProjectQueryService.GetByUrn(urn).ConfigureAwait(false);

			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("GetTransferProjects", Name = "GetTransferProjects")]
		public async Task<ActionResult<AcademyTransferProjectResponse>> GetTransferProjects(
		[FromQuery] string? title,
		[FromQuery] int page = 1,
		[FromQuery] int count = 50,
		[FromQuery] int? urn = null)
		{

			_logger.LogInformation($"Attempting to retrieve {count} Academy Transfer Projects filtered by: urn: {urn} title: {title}", count, urn, title);

			PagedResultResponse<AcademyTransferProjectSummaryResponse> result =
			   await _transferProjectQueryService.GetTransferProjects(page, count, urn, title);

			if (result.Results.Any())
			{
				IEnumerable<string> projectIds = result.Results.Select(p => p.ProjectUrn);
				_logger.LogInformation($"Returning {count} Academy Transfer Projects with Id(s): {projectIds}", result.Results.Count(), string.Join(',', projectIds));
			}

			return Ok(result);
		}
		/// <summary>
		///     Retrieve all projects matching specified filter conditions
		/// </summary>
		/// <param name="searchModel"><see cref="AcademyTransferProjectSummaryResponse"/> describing filtering requirements for the request</param>	
		/// <remarks>
		///     Filters are cumulative (AND logic), applied in the following order: by Status, by URN, by
		///     Delivery Officer.
		/// </remarks>
		/// <response code="200">One or more projects matching the specified filter criteria were found</response>
		/// <response code="404">No projects matched the specified search criteria</response>
		[HttpPost("GetTransferProjects", Name = "GetTransferProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedDataResponse<AcademyTransferProjectSummaryResponse>>> GetProjects(
		GetProjectSearchModel? searchModel)
		{
			PagedDataResponse<AcademyTransferProjectSummaryResponse>? result =
				await _transferProjectQueryService.GetProjects(searchModel!.StatusQueryString, searchModel.TitleFilter,
					searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpDelete("{urn:int}/Delete", Name = "DeleteProject")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteAProjectByUrn(int urn, CancellationToken cancellationToken)
		{
			SetTransferProjectDeletedAtCommand request = new SetTransferProjectDeletedAtCommand(urn);

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => throw new NotImplementedException()
			};
		}

	}
}
