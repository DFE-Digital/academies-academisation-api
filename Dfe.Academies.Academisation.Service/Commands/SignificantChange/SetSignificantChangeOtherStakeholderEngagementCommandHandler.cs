using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeOtherStakeholderEngagementCommandHandler(
	ISignificantChangeProjectRepository repository,
	ILogger<SetSignificantChangeOtherStakeholderEngagementCommandHandler> logger)
	: IRequestHandler<SetSignificantChangeOtherStakeholderEngagementCommand, CommandResult>
{
	public async Task<CommandResult> Handle(
		SetSignificantChangeOtherStakeholderEngagementCommand request,
		CancellationToken cancellationToken)
	{
		var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetOtherStakeholderEngagement(
			request.TrustConsultedStakeholders,
			request.TrustConsultedStakeholdersNotConsultedReason,
			request.WereThereAnyObjectionsRaised,
			request.StakeholderObjectionsDetails,
			request.RelevantStakeholdersNotifiedOfAdmissionsChanges,
			request.RelevantStakeholdersNotifiedOfAdmissionsChangesReason,
			request.SupportingEvidence);

		repository.Update(existingProject);
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
