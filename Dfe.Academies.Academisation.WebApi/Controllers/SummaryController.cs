using Dfe.Academies.Academisation.Domain.Summary;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
    [ApiController]
    [Route("summary")]
    public class SummaryController : ControllerBase
    {
	    private readonly ILogger<SummaryController> _logger;
	    private readonly ISummaryDataService _summaryDataService;

	    public SummaryController(ILogger<SummaryController> logger, ISummaryDataService summaryDataService)
	    {
		    _logger = logger;
		    _summaryDataService = summaryDataService;
	    }

        [HttpGet("projects")]
        public async Task<ActionResult<IEnumerable<ProjectSummary>>> GetProjects(string email, bool? includeConversions, bool? includeTransfers, bool? includeFormAMat, string? searchTerm)
        {
	        _logger.LogInformation($"Attempting to retrieve summary projects");

	        bool noFilters = includeConversions == null && includeTransfers == null && includeFormAMat == null;

			var result = await _summaryDataService.GetProjectSummariesByAssignedEmail(email,
		        includeConversions ?? noFilters,
		        includeTransfers ?? noFilters,
		        includeFormAMat ?? noFilters,
		        searchTerm);

	        return Ok(result);
        }
    }
}
