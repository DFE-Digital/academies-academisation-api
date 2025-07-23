using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Summary;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
    [ApiController]
    [Route("summary")]
    public class SummaryController : ControllerBase
    {
	    private readonly ILogger<SummaryController> _logger;
	    private readonly ISummaryQueryService _summaryQueryService;

	    public SummaryController(ILogger<SummaryController> logger, ISummaryQueryService summaryQueryService)
	    {
		    _logger = logger;
		    _summaryQueryService = summaryQueryService;
	    }

        [HttpGet("projects")]
        public async Task<ActionResult<IEnumerable<ProjectSummary>>> GetProjects(string email, bool? includeConversions, bool? includeTransfers, bool? includeFormAMat, CancellationToken cancellationToken)
        {
	        _logger.LogInformation($"Attempting to retrieve summary projects");

	        bool noFilters = includeConversions == null && includeTransfers == null && includeFormAMat == null;

			var result = await _summaryQueryService.GetProjectSummariesByAssignedEmail(email,
		        includeConversions ?? noFilters,
		        includeTransfers ?? noFilters,
		        includeFormAMat ?? noFilters,
		        cancellationToken);

	        return Ok(result);
        }
    }
}
