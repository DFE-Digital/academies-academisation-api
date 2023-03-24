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
		private readonly IApplicationQueryService _applicationQueryService;
		private readonly IApplicationUpdateCommand _applicationUpdateCommand;
		private readonly ITrustQueryService _trustQueryService;
		private readonly IMediator _mediator;
		private readonly ILogger<ApplicationController> _logger;

		public ApplicationController(IApplicationCreateCommand applicationCreateCommand,
			IApplicationQueryService applicationQueryService,
			IApplicationUpdateCommand applicationUpdateCommand,
			ITrustQueryService trustQueryService,
			IMediator mediator, 
			ILogger<ApplicationController> logger
			)
		{
			// need guard clauses on these check for null
			_applicationCreateCommand = applicationCreateCommand;
			_applicationQueryService = applicationQueryService;
			_applicationUpdateCommand = applicationUpdateCommand;
			_trustQueryService = trustQueryService;
			_mediator = mediator;
			_logger = logger;
		}

		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<ApplicationServiceModel>> Post([FromBody] ApplicationCreateRequestModel request)
		{
			_logger.LogInformation($"Creating application using post endpoint with contributor: {request?.Contributor?.EmailAddress}");
			var result = await _applicationCreateCommand.Execute(request);

			return result switch
			{
				CreateSuccessResult<ApplicationServiceModel> successResult => CreatedAtRoute(GetRouteName, new { id = successResult.Payload.ApplicationId }, successResult.Payload),
				CreateValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{id}", Name = GetRouteName)]
		public async Task<ActionResult<ApplicationServiceModel>> Get(int id)
		{
			_logger.LogInformation($"Getting application, id: {id}");

			var result = await _applicationQueryService.GetById(id);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("contributor/{email}", Name = "List")]
		public async Task<ActionResult<IList<ApplicationServiceModel>>> ListByUser(string email)
		{
			var result = await _applicationQueryService.GetByUserEmail(email);
			return Ok(result);
		}

		[HttpGet("{applicationReference}/applicationReference", Name = "Get")]
		public async Task<ActionResult<ApplicationServiceModel>> Get(string applicationReference)
		{
			var result = await _applicationQueryService.GetByApplicationReference(applicationReference);
			return result != null ? Ok(result) : NotFound();
		}

		[HttpPut("{id}", Name = "Update")]
		public async Task<ActionResult> Update(int id, [FromBody] ApplicationUpdateRequestModel serviceModel)
		{
			_logger.LogInformation($"Updating application: {serviceModel.ApplicationId} with values: {serviceModel.ToString()}");
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
			_logger.LogInformation($"Setting join trust information for application: {applicationId} with details: {command.ToString()}");
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
			_logger.LogInformation($"Setting form trust information for application: {applicationId} with details: {command.ToString()}");
			var result = await _mediator.Send(command with { applicationId = applicationId }, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPost("{applicationId}/form-trust/key-person", Name = "AddKeyPerson")]
		public async Task<ActionResult> AddKeyPerson(int applicationId, [FromBody] CreateTrustKeyPersonCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Adding key people to application: {applicationId} with details: {command.ToString()}");
			var result = await _mediator.Send(command with { ApplicationId = applicationId }, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{applicationId}/form-trust/key-person/{keyPersonId}", Name = "UpdateKeyPerson")]
		public async Task<ActionResult> UpdateKeyPerson(int applicationId, int keyPersonId, [FromBody] UpdateTrustKeyPersonCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Updating key people to application: {applicationId} with details: {command.ToString()}");
			var result = await _mediator.Send(command with { ApplicationId = applicationId, KeyPersonId = keyPersonId }, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpDelete("{applicationId}/form-trust/key-person/{keyPersonId}", Name = "DeleteKeyPerson")]
		public async Task<ActionResult> DeleteKeyPerson(int applicationId, int keyPersonId, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Deleting key person: {keyPersonId} in application: {applicationId}");
			var result = await _mediator.Send(new DeleteTrustKeyPersonCommand(applicationId, keyPersonId), cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpDelete("{applicationId}/form-trust/school/{urn}", Name = "DeleteSchool")]
		public async Task<ActionResult> DeleteSchool(int applicationId, int urn, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Deleting school: {urn} in application: {applicationId}");
			var result = await _mediator.Send(new DeleteSchoolCommand(applicationId, urn), cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{applicationId}/form-trust/key-person/", Name = "GetKeyPeople")]
		public async Task<ActionResult<List<object>>> GetKeyPeople(int applicationId, CancellationToken cancellationToken)
		{
			var result = await _trustQueryService.GetAllTrustKeyPeople(applicationId);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("{applicationId}/form-trust/key-person/{keyPersonId}", Name = "GetKeyPerson")]
		public async Task<ActionResult<object>> GetKeyPerson(int applicationId, int keyPersonId, CancellationToken cancellationToken)
		{
			var result = await _trustQueryService.GetTrustKeyPerson(applicationId, keyPersonId);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpPost("{applicationId:int}/submit", Name = "Submit")]
		public async Task<ActionResult> Submit(int applicationId)
		{
			_logger.LogInformation($"Submitting application: {applicationId}");
			var result = await _mediator.Send(new SubmitApplicationCommand(applicationId)).ConfigureAwait(false);

			return result switch
			{
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				CommandSuccessResult => Ok(),
				CreateValidationErrorResult createValidationErrorResult => BadRequest(createValidationErrorResult.ValidationErrors),
				CreateSuccessResult<LegacyProjectServiceModel> createSuccessResult => CreatedAtRoute("GetLegacyProject", new { id = createSuccessResult.Payload.Id }, createSuccessResult.Payload),
				CreateSuccessResult<IEnumerable<LegacyProjectServiceModel>> createSuccessResult => CreatedAtRoute("GetLegacyProjects", null, createSuccessResult.Payload),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("all", Name = "All")]
		public async Task<ActionResult<List<ApplicationSchoolSharepointServiceModel>>> GetAll()
		{
			var result = await _applicationQueryService.GetAllApplications();
			return Ok(result);
		}
	}
}
