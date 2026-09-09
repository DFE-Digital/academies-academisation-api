using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeReligiousBodyConsultationCommandHandler> logger) : IRequestHandler<SetSignificantChangeReligiousBodyConsultationCommand, CommandResult>
{
	public async Task<CommandResult> Handle(SetSignificantChangeReligiousBodyConsultationCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetReligiousBodyConsultation(
			request.TrustConsultedReligiousBody,
			request.TrustConsultedReligiousBodyNotConsultedReason);

		repository.Update(existingProject);
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}