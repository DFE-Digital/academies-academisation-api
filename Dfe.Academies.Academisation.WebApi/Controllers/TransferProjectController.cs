using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.Application;
using System;

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

		[HttpPut("{urn}/set-trust-information-and-project-dates", Name = "SetTransferProjectTrustInformationAndProjectDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> SetTransferProjectTrustInformationAndProjectDates(int urn,
			[FromBody] SetTransferProjectTrustInformationAndProjectDatesCommand command, CancellationToken cancellationToken)
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

		[HttpGet("{urn}", Name = "GetByUrn")]
		public async Task<ActionResult<AcademyTransferProjectResponse>> GetByUrn(int urn)
		{
			_logger.LogInformation($"Getting transfer project, urn: {urn}");

			var result = await _transferProjectQueryService.GetByUrn(urn).ConfigureAwait(false);

			return result is null ? NotFound() : Ok(result);
		}
		//[HttpGet("{id}", Name = "GetById")]
		//public async Task<ActionResult<AcademyTransferProjectResponse>> GetById(int id)
		//{
		//	_logger.LogInformation($"Getting transfer project, id: {id}");

		//	var result = await _transferProjectQueryService.GetById(id).ConfigureAwait(false);

		//	return result is null ? NotFound() : Ok(result);
		//}
	}
}
