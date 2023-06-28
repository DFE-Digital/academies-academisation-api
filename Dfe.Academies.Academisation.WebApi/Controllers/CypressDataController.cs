using MediatR;
using Microsoft.AspNetCore.Mvc;
using Dfe.Academies.Academisation.Service.Commands.CypressData;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("/cypress-data")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class CypressDataController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CypressDataController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		[Route("add-voluntary-project.cy")]
		public async Task<IActionResult> AddVoluntaryProject([FromBody] CyAddVoluntaryProjectCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
			return Ok(result);
		}
		
	}
}
