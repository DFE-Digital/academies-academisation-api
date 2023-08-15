using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.Application;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("transfer-project")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class TransferProjectController : ControllerBase
	{
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

			return result is null ? BadRequest() : Ok(result);
		}

		[HttpPut("set-rationale", Name = "SetTransferProjectRationale")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectRationale(
			[FromBody] SetTransferProjectRationaleCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project rationale");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("set-transfer-dates", Name = "SetTransferProjectTransferDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectTransferDates(
			[FromBody] SetTransferProjectTransferDatesCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project transfer dates");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("set-legal-requirements", Name = "SetTransferProjectLegalRequirements")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectLegalRequirements(
			[FromBody] SetTransferProjectLegalRequirementsCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Setting transfer project legal requirements");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("set-features", Name = "SetTransferProjectFeatures")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectFeatures(
			[FromBody] SetTransferProjectFeaturesCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project features");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("set-benefits", Name = "SetTransferProjectBenefits")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectBenefits(
		[FromBody] SetTransferProjectBenefitsCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project benefits");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("set-school-additional-data", Name = "SetSchoolAdditionalData")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetSchoolAdditionalData(
			[FromBody] SetTransferringAcademySchoolAdditionalDataCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transferring academy school additional data");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("set-trust-information-and-project-dates", Name = "SetTransferProjectTrustInformationAndProjectDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectTrustInformationAndProjectDates(
			[FromBody] SetTransferProjectTrustInformationAndProjectDatesCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Setting transfer project trust information and project dates");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("assign-user", Name = "AssignUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AssignUser(
			[FromBody] AssignTransferProjectUserCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Assigning user to transfer project");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{urn}", Name = "GetByUrn")]
		public async Task<ActionResult<AcademyTransferProjectResponse>> GetByUrn(int urn)
		{
			_logger.LogInformation($"Getting transfer project, urn: {urn}");

			var result = await _transferProjectQueryService.GetByUrn(urn).ConfigureAwait(false);

			return result is null ? NotFound() : Ok(result);
		}
	}
}
