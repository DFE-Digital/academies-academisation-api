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
		public IActionResult AddFormAMatProject()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("advisory-board-urls.cy")]
		public IActionResult AdvisoryBoardUrls()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("comments-updated-correctly.cy")]
		public IActionResult CommentsUpdatedCorrectly()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("create-approved-decision.cy")]
		public IActionResult CreateApprovedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("create-declined-decision.cy")]
		public IActionResult CreatedDeclinedDecision()
		{
			return Ok();
		}
		
		[HttpPost]
		[Route("create-deferred-decision.cy")]
		public IActionResult CreateDeferredDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("error-handling.cy")]
		public IActionResult ErrorHandling()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-approved-decision.cy")]
		public IActionResult NewPlusEditApprovedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-declined-decision.cy")]
		public IActionResult NewPlusEditDeclinedDecision()
		{
			return Ok();
		}
		[HttpPost]
		[Route("new-plus-edit-deferred-decision.cy")]
		public IActionResult NewPlusEditDeferredDecision()
		{
			return Ok();
		}
	}
}
