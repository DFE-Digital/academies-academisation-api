using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("/conversion-project/advisory-board-decision")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AdvisoryBoardDecisionController : ControllerBase
{
	private const string GetRouteName = "GetProject";
	private readonly IMediator _mediator;
	private readonly IAdvisoryBoardDecisionQueryService _advisoryBoardDecisionQueryService;

	public AdvisoryBoardDecisionController(IMediator mediator, IAdvisoryBoardDecisionQueryService advisoryBoardDecisionQueryService)
	{
		_mediator = mediator;
		_advisoryBoardDecisionQueryService = advisoryBoardDecisionQueryService;
	}

	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> Post([FromBody] AdvisoryBoardDecisionCreateCommand request, CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

		return result switch
		{
			CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel> successResult => CreatedAtRoute(
					GetRouteName,
					new { projectId = successResult.Payload.AdvisoryBoardDecisionId },
					successResult.Payload),
			CreateValidationErrorResult validationErrorResult =>
				new BadRequestObjectResult(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	[HttpGet("{projectId:int}", Name = GetRouteName)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> GetByProjectId(int projectId)
	{
		var result = await _advisoryBoardDecisionQueryService.GetByProjectId(projectId);

		return result is null
			? NotFound()
			: new OkObjectResult(result);
	}

	[HttpGet("~/transfer-project/advisory-board-decision/{projectId:int}", Name = "GetTransferAdvisoryBoardDecisionByProjectId")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> GetByTransferProjectId(int projectId)
	{
		var result = await _advisoryBoardDecisionQueryService.GetByProjectId(projectId, true);

		return result is null
			? NotFound()
			: new OkObjectResult(result);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> Put([FromBody] AdvisoryBoardDecisionUpdateCommand request, CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(request, cancellationToken);

		return result switch
		{
			CommandSuccessResult => new OkResult(),
			NotFoundCommandResult => new NotFoundResult(),
			BadRequestCommandResult => new BadRequestResult(),
			CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

}
