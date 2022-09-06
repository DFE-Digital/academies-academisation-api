using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.LegalRequirement;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("/conversion-project/legal-requirements")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class LegalRequirementController : ControllerBase
{
	private readonly ILegalRequirementGetQuery _legalRequirementGetQuery;
	private readonly ILegalRequirementCreateCommand _legalRequirementCreateCommand;
	private readonly ILegalRequirementUpdateCommand _legalRequirementUpdateCommand;

	public LegalRequirementController(ILegalRequirementGetQuery legalRequirementGetQuery, ILegalRequirementCreateCommand legalRequirementCreateCommand,
		ILegalRequirementUpdateCommand legalRequirementUpdateCommand)
	{
		_legalRequirementGetQuery = legalRequirementGetQuery;
		_legalRequirementCreateCommand = legalRequirementCreateCommand;
		_legalRequirementUpdateCommand = legalRequirementUpdateCommand;
	}

	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]	
	[HttpPost]
	public async Task<ActionResult<LegalRequirementServiceModel>> Create([FromBody] LegalRequirementServiceModel request)
	{
		var result = await _legalRequirementCreateCommand.Execute(request);

		return result switch
		{
			CreateSuccessResult<LegalRequirementServiceModel> successResult => CreatedAtRoute(HttpMethods.Get,
					new { successResult.Payload.Id }, successResult.Payload),

			CreateValidationErrorResult<LegalRequirementServiceModel> validationErrorResult =>
				new BadRequestObjectResult(validationErrorResult.ValidationErrors),

			_ => throw new NotImplementedException($"Result type not expected - {result.GetType()}")
		};
	}

	[HttpGet("{projectId:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<LegalRequirementServiceModel>> Get(int projectId)
	{
		var result = await _legalRequirementGetQuery.Execute(projectId);

		return result is null
			? NotFound()
			: new OkObjectResult(result);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> Update([FromBody] LegalRequirementServiceModel request)
	{
		var result = await _legalRequirementUpdateCommand.Execute(request);

		return result switch
		{
			CommandSuccessResult => new OkResult(),
			NotFoundCommandResult => new NotFoundResult(),
			BadRequestCommandResult => new BadRequestResult(),
			CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
			_ => throw new NotImplementedException($"Result type not expected - ({result.GetType()}")
		};
	}
}
