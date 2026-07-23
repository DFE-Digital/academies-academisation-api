using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("significant-change")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class SignificantChangeController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SignificantChangeController> _logger;

		public SignificantChangeController(IMediator mediator, ILogger<SignificantChangeController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost(Name = "CreateSignificantProject")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<SignificantChangeProjectResponse>> CreateSignificantProject(
			[FromBody] CreateSignificantProjectCommand command,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating significant change project");
			var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

			return result switch
			{
				CreateSuccessResult<SignificantChangeProjectResponse> successResult => Created($"/significant-change/{successResult.Payload.Urn}", successResult.Payload),
				null => BadRequest(),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPost("search", Name = "GetSignificantChangeProjects")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<PagedDataResponse<SignificantChangeProjectSearchResponse>>> GetSignificantChangeProjects(
			[FromBody] GetSignificantProjectsQuery query,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting significant change projects");
			var result = await _mediator.Send(query, cancellationToken);

			return result switch
			{
				PagedDataResponse<SignificantChangeProjectSearchResponse> pagedResult => Ok(pagedResult)
			};
		}

		[HttpGet("project/{id:int}", Name = "GetSignificantProjectById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<SignificantChangeProjectSearchResponse>> GetSignificantChangeProject(int id,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting significant change project with id {id}", id);
			var result = await _mediator.Send(new GetSignificantProjectByIdQuery(id), cancellationToken);

			return result is null ? NotFound() : Ok(result);
		}
	}
}
