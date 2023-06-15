using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers.Cypress
{
	[Route("cypress")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class CypressController : ControllerBase
	{
		public CypressController()
		{
		}

		[Route("add-form-a-mat-project.cy")]
		public IActionResult Add_form_a_mat_project()
		{
			return Ok();
		}
	}
}
