using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("significant-change")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class SignificantChangeController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SignificantChangeController> _logger;

		public SignificantChangeController(IMediator mediator, ILogger<SignificantChangeController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPut("{id:int}/SetAssignedUser", Name = "SetAssignedUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetAssignedUser(
			int id,
			[FromBody] SetAssignedUserCommand request)
		{
			// Ensure the command's ID matches the route parameter
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

		[HttpPost(Name = "CreateSignificantProject")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<SignificantChangeProjectResponse>> CreateSignificantProject(
			[FromBody] CreateSignificantProjectCommand command,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating significant change project");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CreateSuccessResult<SignificantChangeProjectResponse> successResult => Created($"/significant-change/{successResult.Payload.Urn}", successResult.Payload),
				null => BadRequest(),
				_ => throw new NotImplementedException()
			};
		}
	}
}