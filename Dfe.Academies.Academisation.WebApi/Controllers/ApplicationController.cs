using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application.Trust;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("application")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ApplicationController(
		IApplicationQueryService applicationQueryService,
		ITrustQueryService trustQueryService,
		IMediator mediator,
		ILogger<ApplicationController> logger
			) : ControllerBase
	{
		private const string GetRouteName = "GetApplication";

		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<ApplicationServiceModel>> Post([FromBody] ApplicationCreateCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Creating application using post endpoint with contributor: {command?.Contributor?.EmailAddress}");

			var result = await mediator.Send(command!, cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Getting application, id: {id}");

			var result = await applicationQueryService.GetById(id);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("contributor/{email}", Name = "List")]
		public async Task<ActionResult<IList<ApplicationServiceModel>>> ListByUser(string email)
		{
			var result = await applicationQueryService.GetByUserEmail(email);
			return Ok(result);
		}

		[HttpGet("{applicationReference}/applicationReference", Name = "Get")]
		public async Task<ActionResult<ApplicationServiceModel>> Get(string applicationReference)
		{
			var result = await applicationQueryService.GetByApplicationReference(applicationReference);
			return result != null ? Ok(result) : NotFound();
		}

		[HttpPut("{id}", Name = "Update")]
		public async Task<ActionResult> Update(int id, [FromBody] ApplicationUpdateCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Updating application: {command.ApplicationId} with values: {command.ToString()}");
			CommandResult? result = null;

			// this should probably be a pipeline validator
			if (id != command.ApplicationId)
			{
				result = new CommandValidationErrorResult(
					new List<ValidationError>() {
						new("Id", "Ids must be the same")
					});
			}
			else { 
				result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
			}

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
			logger.LogInformation($"Setting join trust information for application: {applicationId} with details: {command.ToString()}");
			var result = await mediator.Send(command with { applicationId = applicationId}, cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Setting form trust information for application: {applicationId} with details: {command.ToString()}");
			var result = await mediator.Send(command with { applicationId = applicationId }, cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Adding key people to application: {applicationId} with details: {command.ToString()}");
			var result = await mediator.Send(command with { ApplicationId = applicationId }, cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Updating key people to application: {applicationId} with details: {command.ToString()}");
			var result = await mediator.Send(command with { ApplicationId = applicationId, KeyPersonId = keyPersonId }, cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Deleting key person: {keyPersonId} in application: {applicationId}");
			var result = await mediator.Send(new DeleteTrustKeyPersonCommand(applicationId, keyPersonId), cancellationToken).ConfigureAwait(false);

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
			logger.LogInformation($"Deleting school: {urn} in application: {applicationId}");
			var result = await mediator.Send(new DeleteSchoolCommand(applicationId, urn), cancellationToken).ConfigureAwait(false);

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
			var result = await trustQueryService.GetAllTrustKeyPeople(applicationId);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("{applicationId}/form-trust/key-person/{keyPersonId}", Name = "GetKeyPerson")]
		public async Task<ActionResult<object>> GetKeyPerson(int applicationId, int keyPersonId, CancellationToken cancellationToken)
		{
			var result = await trustQueryService.GetTrustKeyPerson(applicationId, keyPersonId);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpPost("{applicationId:int}/submit", Name = "Submit")]
		public async Task<ActionResult> Submit(int applicationId)
		{
			logger.LogInformation($"Submitting application: {applicationId}");
			var result = await mediator.Send(new ApplicationSubmitCommand(applicationId)).ConfigureAwait(false);

			return result switch
			{
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				CommandSuccessResult => Ok(),
				CreateValidationErrorResult createValidationErrorResult => BadRequest(createValidationErrorResult.ValidationErrors),
				CreateSuccessResult<ConversionProjectServiceModel> createSuccessResult => CreatedAtRoute("GetLegacyProject", new { id = createSuccessResult.Payload.Id }, createSuccessResult.Payload),
				CreateSuccessResult<IEnumerable<ConversionProjectServiceModel>> createSuccessResult => CreatedAtRoute("GetLegacyProjects", null, createSuccessResult.Payload),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("all", Name = "All")]
		public async Task<ActionResult<List<ApplicationSchoolSharepointServiceModel>>> GetAll()
		{
			var result = await applicationQueryService.GetAllApplications();
			return Ok(result);
		}

		[HttpDelete("{applicationId}/delete-application", Name = "DeleteApplication")]
		public async Task<ActionResult> DeleteApplication(int applicationId)
		{
			logger.LogInformation($"Deleting application: {applicationId}");
			
			var result = await mediator.Send(new ApplicationDeleteCommand(applicationId)).ConfigureAwait(false);

				return result switch
			{
				CommandSuccessResult => Ok($"Application:{applicationId} successfully deleted"),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
	}

		
}
