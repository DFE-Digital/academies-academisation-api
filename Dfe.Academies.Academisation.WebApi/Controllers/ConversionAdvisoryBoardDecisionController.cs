﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("/conversion-project/advisory-board-decision")]
[ApiController]
public class ConversionAdvisoryBoardDecisionController : ControllerBase
{
    private readonly IAdvisoryBoardDecisionCreateCommand _decisionCreateCommand;
    private readonly IConversionAdvisoryBoardDecisionGetQuery _decisionGetQuery;

    public ConversionAdvisoryBoardDecisionController(IAdvisoryBoardDecisionCreateCommand decisionCreateCommand, IConversionAdvisoryBoardDecisionGetQuery decisionGetQuery)
    {
        _decisionCreateCommand = decisionCreateCommand;
        _decisionGetQuery = decisionGetQuery;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> Post([FromBody] AdvisoryBoardDecisionCreateRequestModel request)
    {
        var result = await _decisionCreateCommand.Execute(request);

        return result switch
        {
            CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel> successResult => CreatedAtRoute(
                    HttpMethods.Get, 
                    new { Id = successResult.Payload.AdvisoryBoardDecisionId}, 
                    successResult.Payload),
            CreateValidationErrorResult<ConversionAdvisoryBoardDecisionServiceModel> validationErrorResult =>
                new BadRequestObjectResult(validationErrorResult.ValidationErrors),
            _ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
        };
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversionAdvisoryBoardDecisionServiceModel>> GetByProjectId(int projectId)
    {
        var result = await _decisionGetQuery.Execute(projectId);

        return result is null
            ? NotFound()
            : new OkObjectResult(result);
    }
}
