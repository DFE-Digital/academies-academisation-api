using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("conversion-project/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ConversionProjectController : ControllerBase
	{
		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly IMediator _mediator;

		public ConversionProjectController( IConversionProjectQueryService conversionProjectQueryService,
									   IMediator mediator)
		{

			_conversionProjectQueryService = conversionProjectQueryService;
			_mediator = mediator;
		}

		/// <summary>
		/// Updates the project with the specified id sets external application form data using the data in the command <paramref name="request"/>
		/// </summary>
		/// <param name="id">The ID of the project to update</param>
		/// <param name="request">the command containing the payload of updates</param>
		/// <exception cref="NotImplementedException"></exception>
		/// <response code="200">The update was applied successfully</response>
		/// <response code="400">The request failed validation and the errors are returned</response>
		/// <response code="404">The Project with the specified ID was not found</response>
		[HttpPut("{id:int}/SetExternalApplicationForm", Name = "SetExternalApplicationForm")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetExternalApplicationForm(
			int id,
			SetExternalApplicationFormCommand request)
		{
			request.Id = id;

			CommandResult result = await _mediator.Send(request);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
	}
}
