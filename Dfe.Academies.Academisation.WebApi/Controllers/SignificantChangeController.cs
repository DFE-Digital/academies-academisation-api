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