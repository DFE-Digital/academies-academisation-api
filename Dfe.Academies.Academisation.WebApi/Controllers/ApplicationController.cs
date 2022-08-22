using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("application")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ApplicationController : ControllerBase
	{
		private readonly IApplicationCreateCommand _applicationCreateCommand;
		private readonly IApplicationGetQuery _applicationGetQuery;
		private readonly IApplicationUpdateCommand _applicationUpdateCommand;
		private readonly IApplicationSubmitCommand _applicationSubmitCommand;

		public ApplicationController(IApplicationCreateCommand applicationCreateCommand,
			IApplicationGetQuery applicationGetQuery,
			IApplicationUpdateCommand applicationUpdateCommand,
			IApplicationSubmitCommand applicationSubmitCommand
			)
		{
			_applicationCreateCommand = applicationCreateCommand;
			_applicationGetQuery = applicationGetQuery;
			_applicationUpdateCommand = applicationUpdateCommand;
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
				CreateValidationErrorResult<ApplicationServiceModel> validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpGet("{id}", Name = "Get")]
		public async Task<ActionResult<ApplicationServiceModel>> Get(int id)
		{
			var result = await _applicationGetQuery.Execute(id);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpPut("{id}", Name = "Update")]
		public async Task<ActionResult<ApplicationServiceModel>> Update(int id, [FromBody] ApplicationServiceModel serviceModel)
		{
			var result = await _applicationUpdateCommand.Execute(id, serviceModel);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPost("submit/{id}", Name = "Submit")]
		public async Task<ActionResult> Submit(int id)
		{
			var result = await _applicationSubmitCommand.Execute(id);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
	}
}
