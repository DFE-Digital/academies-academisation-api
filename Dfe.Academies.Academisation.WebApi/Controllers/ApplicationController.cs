using System.Reflection;
using System.Threading;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("application")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ApplicationController : ControllerBase
	{
		private const string GetRouteName = "GetApplication";
		private readonly IApplicationCreateCommand _applicationCreateCommand;
		private readonly IApplicationGetQuery _applicationGetQuery;
		private readonly IApplicationUpdateCommand _applicationUpdateCommand;
		private readonly IApplicationSubmitCommand _applicationSubmitCommand;
		private readonly IApplicationListByUserQuery _applicationsListByUserQuery;
		private readonly IMediator _mediator;
		private readonly ILogger<ApplicationController> _logger;

		public ApplicationController(IApplicationCreateCommand applicationCreateCommand,
			IApplicationGetQuery applicationGetQuery,
			IApplicationUpdateCommand applicationUpdateCommand,
			IApplicationSubmitCommand applicationSubmitCommand,
			IApplicationListByUserQuery applicationsListByUserQuery,
			IMediator mediator, 
			ILogger<ApplicationController> logger
			)
		{
			// need guard clauses on these check for null
			_applicationCreateCommand = applicationCreateCommand;
			_applicationGetQuery = applicationGetQuery;
			_applicationUpdateCommand = applicationUpdateCommand;
			_applicationSubmitCommand = applicationSubmitCommand;
			_applicationsListByUserQuery = applicationsListByUserQuery;
			_mediator = mediator;
			_logger = logger;
		}

		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<ApplicationServiceModel>> Post([FromBody] ApplicationCreateRequestModel request)
		{
			var result = await _applicationCreateCommand.Execute(request);

			return result switch
			{
				CreateSuccessResult<ApplicationServiceModel> successResult => CreatedAtRoute(GetRouteName, new { id = successResult.Payload.ApplicationId }, successResult.Payload),
				CreateValidationErrorResult<ApplicationServiceModel> validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{id}", Name = GetRouteName)]
		public async Task<ActionResult<ApplicationServiceModel>> Get(int id)
		{
			// basic log line to check logger is working
			_logger.LogInformation($"Getting application, id: {id}");

			var result = await _applicationGetQuery.Execute(id);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("contributor/{email}", Name = "List")]
		public async Task<ActionResult<IList<ApplicationServiceModel>>> ListByUser(string email)
		{
			var result = await _applicationsListByUserQuery.Execute(email);
			return Ok(result);
		}

		[HttpPut("{id}", Name = "Update")]
		public async Task<ActionResult> Update(int id, [FromBody] ApplicationUpdateRequestModel serviceModel)
		{
			var result = await _applicationUpdateCommand.Execute(id, serviceModel);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{applicationId}/join-trust", Name = "SetJoinTrustDetails")]
		public async Task<ActionResult> SetJoinTrustDetails(int applicationId, [FromBody] SetJoinTrustDetailsCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command with { applicationId = applicationId}, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}


		[HttpPut("{applicationId}/form-trust", Name = "SetFormTrustDetails")]
		public async Task<ActionResult> SetFormTrustDetails(int applicationId, [FromBody] SetFormTrustDetailsCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command with { applicationId = applicationId }, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPost("submit/{id:int}", Name = "Submit")]
		public async Task<ActionResult> Submit(int id)
		{
			var result = await _applicationSubmitCommand.Execute(id);

			return result switch
			{
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				CommandSuccessResult => Ok(),
				CreateValidationErrorResult<LegacyProjectServiceModel> createValidationErrorResult => BadRequest(createValidationErrorResult.ValidationErrors),
				CreateSuccessResult<LegacyProjectServiceModel> createSuccessResult => CreatedAtRoute("GetLegacyProject", new { id = createSuccessResult.Payload.Id }, createSuccessResult.Payload),
				_ => throw new NotImplementedException()
			};
		}
	}
}
