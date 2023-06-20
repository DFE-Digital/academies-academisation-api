using Dfe.Academies.Academisation.Service.Commands.CypressData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
		[Route("add-form-a-mat-project.cy")]
		public async Task<IActionResult> AddFormAMatProject()
		{
			var result = _mediator.Send(new CyAddFormAMatProjectCommand());
			return Ok();
		}
		
		[HttpPost]
		[Route("advisory-board-urls.cy")]
		public async Task<IActionResult> AdvisoryBoardUrls()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("comments-updated-correctly.cy")]
		public async Task<IActionResult> CommentsUpdatedCorrectly()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("create-approved-decision.cy")]
		public async Task<IActionResult> CreateApprovedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("create-declined-decision.cy")]
		public async Task<IActionResult> CreatedDeclinedDecision()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("create-deferred-decision.cy")]
		public async Task<IActionResult> CreateDeferredDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("error-handling.cy")]
		public async Task<IActionResult> ErrorHandling()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-approved-decision.cy")]
		public async Task<IActionResult> NewPlusEditApprovedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-declined-decision.cy")]
		public async Task<IActionResult> NewPlusEditDeclinedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-deferred-decision.cy")]
		public async Task<IActionResult> NewPlusEditDeferredDecision()
		{
			return Ok();
		}
	}
}
