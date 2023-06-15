using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("/cypress-data")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class CypressDataController : ControllerBase
	{
		public CypressDataController()
		{
		}

		[HttpPost]
		[Route("add-form-a-mat-project.cy")]
		public IActionResult Add_form_a_mat_project()
		{
			return Ok();
		}
	}
}
