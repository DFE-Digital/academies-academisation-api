using System.Net;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("school")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class SchoolController : ControllerBase
	{
		private readonly ILogger<SchoolController> _logger;
		private readonly ICreateLoanCommandHandler _createLoanCommandHandler;
		private readonly IUpdateLoanCommandHandler _updateLoanCommandHandler;
		private readonly IDeleteLoanCommandHandler _deleteLoanCommandHandler;
		private readonly ICreateLeaseCommandHandler _createLeaseCommandHandler;
		private readonly IUpdateLeaseCommandHandler _updateLeaseCommandHandler;
		private readonly IDeleteLeaseCommandHandler _deleteLeaseCommandHandler;

		public SchoolController(ILogger<SchoolController> logger,
			ICreateLoanCommandHandler createLoanCommandHandler,
			IUpdateLoanCommandHandler updateLoanCommandHandler,
			IDeleteLoanCommandHandler deleteLoanCommandHandler, 
			ICreateLeaseCommandHandler createLeaseCommandHandler, 
			IUpdateLeaseCommandHandler updateLeaseCommandHandler,
			IDeleteLeaseCommandHandler deleteLeaseCommandHandler)
		{
			_logger = logger;
			_createLoanCommandHandler = createLoanCommandHandler;
			_updateLoanCommandHandler = updateLoanCommandHandler;
			_deleteLoanCommandHandler = deleteLoanCommandHandler;
			_createLeaseCommandHandler = createLeaseCommandHandler;
			_updateLeaseCommandHandler = updateLeaseCommandHandler;
			_deleteLeaseCommandHandler = deleteLeaseCommandHandler;
		}

		[HttpPost("school/loan/update", Name = "UpdateLoan")]
		public async Task<IActionResult> UpdateLoan([FromBody] UpdateLoanCommand command)
		{
			var result = await _updateLoanCommandHandler.Handle(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		
		[HttpPut("school/loan/create", Name = "CreateLoan")]
		public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command)
		{
			var result = await _createLoanCommandHandler.Handle(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpDelete("school/loan/delete", Name = "DeleteLoan")]
		public async Task<IActionResult> DeleteLoan([FromBody] DeleteLoanCommand command)
		{
			var result = await _deleteLoanCommandHandler.Handle(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPost("school/lease/update", Name = "UpdateLease")]
		public async Task<IActionResult> UpdateLease([FromBody] UpdateLeaseCommand command)
		{
			var result = await _updateLeaseCommandHandler.Handle(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		
		[HttpPut("school/lease/create", Name = "CreateLease")]
		public async Task<IActionResult> CreateLease([FromBody] CreateLeaseCommand command)
		{
			var result = await _createLeaseCommandHandler.Handle(command);
			
			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult => BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpDelete("school/lease/delete", Name = "DeleteLease")]
		public async Task<IActionResult> DeleteLease([FromBody] DeleteLeaseCommand command)
		{
			var result = await _deleteLeaseCommandHandler.Handle(command);
			
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
