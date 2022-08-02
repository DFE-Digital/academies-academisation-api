using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.Commands;
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
		private readonly IApplicationSubmitCommand _applicationSubmitCommand;

		public ConversionApplicationController(IApplicationCreateCommand applicationCreateCommand, IApplicationGetQuery applicationGetQuery, IApplicationSubmitCommand applicationSubmitCommand)
		{
			_applicationCreateCommand = applicationCreateCommand;
			_applicationGetQuery = applicationGetQuery;
			_applicationSubmitCommand = applicationSubmitCommand;
		}

		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

		[HttpGet("{id}", Name = "Get")]
		public async Task<ApplicationServiceModel> Get(int id)
		{
			return await _applicationGetQuery.Execute(id);
		}

		[HttpPost("submit/{id}", Name = "Submit")]
		public async Task<ActionResult> Submit(int id)
		{
			var result = await _applicationSubmitCommand.Execute(id);

			return result switch
			{
				CommandSuccessResult => Ok(),
				CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
	}
}
