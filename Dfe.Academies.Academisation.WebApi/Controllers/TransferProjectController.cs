﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Dfe.Academies.Academisation.IService.Query;

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

		public TransferProjectController(IMediator mediator,ITransferProjectQueryService transferProjectQueryService,
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
			[FromBody]CreateTransferProjectCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Creating transfer project");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result is null ? BadRequest() : Ok(result);
		}

		[HttpPut(Name = "SetTransferProjectRationale")]
		[ProducesResponseType(StatusCodes.Status201Created)]
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

		[HttpGet("{urn}", Name = "GetByUrn")]
		public async Task<ActionResult<AcademyTransferProjectResponse>> GetByUrn(int urn)
		{
			_logger.LogInformation($"Getting transfer project, urn: {urn}");

			var result = await _transferProjectQueryService.GetByUrn(urn);
			
			return result is null ? NotFound() : Ok(result);
		}


	}
}
