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
		[HttpPost]
		[Route("add-sponsored-project.cy")]
		public async Task<IActionResult> AddSponsoredProject([FromBody] CyAddSponsoredProjectCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
			return Ok(result);
		}
		[HttpPost]
		[Route("add-form-a-mat-project.cy")]
		public async Task<IActionResult> AddFormAMatProject([FromBody] CyAddFormAMatProjectCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
			return Ok(result);
		}

		[HttpPost]
		[Route("add-project-from-A2B.cy")]
		public async Task<IActionResult> AddProjectFromA2B([FromBody] CyAddProjectFromA2BCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
			return Ok(result);
		}

	}
}
