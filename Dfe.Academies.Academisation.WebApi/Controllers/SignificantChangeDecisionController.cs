using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("/significant-change/decision")]
	[ApiController]
    public class SignificantChangeDecisionController(IMediator mediator) : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult<SignificantChangeDecisionServiceModel>> Post([FromBody] SignificantChangeDecisionCommand request, CancellationToken cancellationToken)
        {
            var validationFailure = await ValidateProjectAsync(request.SignificantChangeProjectId, cancellationToken);

            if (validationFailure is not null)
            {
                return validationFailure;
            }

            var result = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

            if (result is CreateSuccessResult<SignificantChangeDecisionServiceModel>)
            {
                await UpdateStatusAsync(request.SignificantChangeProjectId!.Value, request.Decision, request.ApprovedConditionsSet, cancellationToken);
            }

            return result switch
            {
                CreateSuccessResult<SignificantChangeDecisionServiceModel> successResult => CreatedAtRoute(
                    "GetProject",
                    new { projectId = successResult.Payload.AdvisoryBoardDecisionId },
                    successResult.Payload),
                CreateValidationErrorResult validationErrorResult =>
                    new BadRequestObjectResult(validationErrorResult.ValidationErrors),
                _ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()})")
            };
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put([FromBody] SignificantChangeUpdateDecisionCommand request, CancellationToken cancellationToken)
        {
            var validationFailure = await ValidateProjectAsync(request.SignificantChangeProjectId, cancellationToken);

            if (validationFailure is not null)
            {
                return validationFailure;
            }

            var result = await mediator.Send(request, cancellationToken);

            if (result is CommandSuccessResult)
            {
                await UpdateStatusAsync(request.SignificantChangeProjectId!.Value, request.Decision, request.ApprovedConditionsSet, cancellationToken);
            }

            return result switch
            {
                CommandSuccessResult => new OkResult(),
                NotFoundCommandResult => new NotFoundResult(),
                BadRequestCommandResult => new BadRequestResult(),
                CommandValidationErrorResult validationErrorResult => new BadRequestObjectResult(validationErrorResult.ValidationErrors),
                _ => throw new NotImplementedException($"Other CommandResult types not expected ({result.GetType()})")
            };
        }

        [HttpGet("{projectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SignificantChangeDecisionServiceModel>> GetByProjectId(int projectId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSignificantChangeDecisionQuery(projectId), cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Validates the project exists and all tasks are complete.
        /// Returns a failure result, or null when validation passes.
        /// </summary>
        private async Task<ActionResult?> ValidateProjectAsync(int? projectId, CancellationToken cancellationToken)
        {
            if (projectId is null)
            {
                return new BadRequestObjectResult("SignificantChangeProjectId must not be null");
            }

            var sigChangeProject = await mediator.Send(new GetSignificantChangeProjectByIdQuery(projectId.Value), cancellationToken);

            if (sigChangeProject is null)
            {
                return new NotFoundResult();
            }

            if (!sigChangeProject.AllTasksComplete())
            {
                return new BadRequestObjectResult("All tasks must be complete before a decision can be made");
            }

            if (sigChangeProject.AssignedUser is null)
            {
                return new BadRequestObjectResult("Project must have an assigned user");
			}

            return null;
        }

        private Task<CommandResult> UpdateStatusAsync(int projectId, Decision decision, bool? approvedConditionsSet, CancellationToken cancellationToken) =>
            mediator.Send(new SignificantChangeStatusCommand(projectId, decision, approvedConditionsSet), cancellationToken);
    }
}
