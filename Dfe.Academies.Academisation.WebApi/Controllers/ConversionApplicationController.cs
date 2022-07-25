using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConversionApplicationController : ControllerBase
	{
		private readonly IApplicationCreateCommand _applicationCreateCommand;
		private readonly IApplicationGetQuery _applicationGetQuery;

		public ConversionApplicationController(IApplicationCreateCommand applicationCreateCommand, IApplicationGetQuery applicationGetQuery)
		{
			_applicationCreateCommand = applicationCreateCommand;
			_applicationGetQuery = applicationGetQuery;
		}

		[HttpPost]
		public async Task<ApplicationServiceModel> Post([FromBody] ApplicationCreateRequestModel request)
		{

			var result = await _applicationCreateCommand.Execute(request);

			if (result is not CreateSuccessResult<ApplicationServiceModel> successResult)
			{
				// ToDo: map validation errors to HTTP 400
				throw new NotImplementedException();
			}

			return successResult.Payload;
		}

		[HttpGet]
		public async Task<ApplicationServiceModel> Get(int id)
		{
			return await _applicationGetQuery.Execute(id);
		}
	}
}
