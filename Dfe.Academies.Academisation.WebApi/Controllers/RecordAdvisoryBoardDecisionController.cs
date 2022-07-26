using Dfe.Academies.Academisation.IService.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[Route("api/conversion-project/[controller]")]
[ApiController]
public class RecordAdvisoryBoardDecisionController : ControllerBase
{
    public RecordAdvisoryBoardDecisionController()
    {
    }

    [HttpPost]
    public async Task Post([FromBody] AdvisoryBoardDecisionCreateRequestModel request)
    {
        await Task.CompletedTask;
    }
}