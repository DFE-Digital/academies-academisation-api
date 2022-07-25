using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("api/conversion-project/AdvisoryBoardDecision")]
[ApiController]
[ProducesResponseType(201)]
public class ConversionAdvisoryBoardDecisionController : ControllerBase
{
    private readonly IAdvisoryBoardDecisionCreateCommand _createCommand;

    public ConversionAdvisoryBoardDecisionController(IAdvisoryBoardDecisionCreateCommand createCommand)
    {
        _createCommand = createCommand;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AdvisoryBoardDecisionCreateRequestModel request)
    {
        await _createCommand.Execute(request);

        return new CreatedResult(string.Empty, null);
    }
}
