using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConversionApplicationController : ControllerBase
	{
		public ConversionApplicationController(IApplicationCreateCommand applicationCreateCommand)
		{
			_applicationCreateCommand = applicationCreateCommand;
		}

		IApplicationCreateCommand _applicationCreateCommand;

		// POST api/<ConversionApplicationController>
		[HttpPost]
		public async Task Post([FromBody] ApplicationCreateRequestModel request)
		{
			await _applicationCreateCommand.Create(request);
		}
	}
}
