using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("conversion-application")]
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

		[ProducesResponseType(StatusCodes.Status201Created)]
		[HttpPost]
		public async Task<ActionResult<ApplicationServiceModel>> Post([FromBody] ApplicationCreateRequestModel request)
		{
			var result = await _applicationCreateCommand.Execute(request);

			return result switch
			{
				CreateSuccessResult<ApplicationServiceModel> successResult => CreatedAtRoute("Get", new { id = successResult.Payload.ApplicationId }, successResult.Payload),
				CreateValidationErrorResult<ApplicationServiceModel> validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{id}", Name="Get")]
		public async Task<ApplicationServiceModel> Get(int id)
		{
			return await _applicationGetQuery.Execute(id);
		}
	}
}
