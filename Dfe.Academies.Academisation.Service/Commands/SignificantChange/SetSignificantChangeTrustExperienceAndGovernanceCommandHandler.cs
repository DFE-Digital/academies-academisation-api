using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeTrustExperienceAndGovernanceCommandHandler(
	ISignificantChangeProjectRepository repository,
	ILogger<SetSignificantChangeTrustExperienceAndGovernanceCommandHandler> logger)
	: IRequestHandler<SetSignificantChangeTrustExperienceAndGovernanceCommand, CommandResult>
{
	public async Task<CommandResult> Handle(SetSignificantChangeTrustExperienceAndGovernanceCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetTrustExperienceAndGovernance(request.Answer, request.AdditionalInformation, request.SupportingEvidenceSharepointLink);

		repository.Update(existingProject);
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
