using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision
{
	public class SignificantChangeStatusCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SignificantChangeStatusCommandHandler> logger) : IRequestHandler<SignificantChangeStatusCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SignificantChangeStatusCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

			if (existingProject is null)
			{
				logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
				return new NotFoundCommandResult();
			}

			var status = DetermineStatus(request.Status, request.ApprovedWithConditions ?? false);

			existingProject.SetStatus(status);

			repository.Update(existingProject);
			await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}

		private static SignificantChangeStatus DetermineStatus(Decision decision, bool approvedWithConditions)
		{
			return decision switch
			{
				Decision.Approved when approvedWithConditions => SignificantChangeStatus.ApprovedWithConditions,
				Decision.Approved => SignificantChangeStatus.Approved,
				Decision.Deferred => SignificantChangeStatus.Deferred,
				Decision.Declined => SignificantChangeStatus.Declined,
				Decision.Withdrawn => SignificantChangeStatus.Withdrawn,
				_ => SignificantChangeStatus.PreDecision
			};
		}
	}
}
