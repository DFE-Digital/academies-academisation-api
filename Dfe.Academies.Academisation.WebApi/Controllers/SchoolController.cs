using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("school")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class SchoolController : ControllerBase
	{
		private readonly ILogger<SchoolController> _logger;
		private readonly IMediator _mediator;

		public SchoolController(ILogger<SchoolController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		[HttpPost("loan/update", Name = "UpdateLoan")]
		public async Task<IActionResult> UpdateLoan([FromBody] UpdateLoanCommand command)
		{
			try
			{
				var result = await _mediator.Send(command);
				return result switch
				{
					CommandSuccessResult => Ok(),
					NotFoundCommandResult => NotFound(),
					_ => new InternalServerErrorObjectResult("Error serving request")
				};
			}
			catch (Exception ex)
			{
				return default;
			}
		}
		
		[HttpPut("loan/create", Name = "CreateLoan")]
		public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpDelete("loan/delete", Name = "DeleteLoan")]
		public async Task<IActionResult> DeleteLoan([FromBody] DeleteLoanCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpPost("lease/update", Name = "UpdateLease")]
		public async Task<IActionResult> UpdateLease([FromBody] UpdateLeaseCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}
		
		[HttpPut("lease/create", Name = "CreateLease")]
		public async Task<IActionResult> CreateLease([FromBody] CreateLeaseCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}

		[HttpDelete("lease/delete", Name = "DeleteLease")]
		public async Task<IActionResult> DeleteLease([FromBody] DeleteLeaseCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}


		[HttpPut("additional-details", Name = "SetAdditionalDetails")]
		public async Task<IActionResult> SetAdditionalDetails([FromBody] SetAdditionalDetailsCommand command)
		{
			var result = await _mediator.Send(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				_ => new InternalServerErrorObjectResult("Error serving request")
			};
		}
	}
}
