using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("/significant-change/decision")]
	[ApiController]
	public class SignificantChangeDecisionController(IMediator mediator) : ControllerBase
	{
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> Post([FromBody] SignificantChangeDecisionCommand request, CancellationToken cancellationToken)
		{
			var result = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel> successResult => CreatedAtRoute(
					"GetProject",
					new { projectId = successResult.Payload.AdvisoryBoardDecisionId },
					successResult.Payload),
				CreateValidationErrorResult validationErrorResult =>
					new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
			};
		}
	}
}
