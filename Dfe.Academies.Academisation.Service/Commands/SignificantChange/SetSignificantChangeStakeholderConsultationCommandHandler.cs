using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeStakeholderConsultationCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeStakeholderConsultationCommandHandler> logger) : IRequestHandler<SetSignificantChangeStakeholderConsultationCommand, CommandResult>
{
	private readonly ISignificantChangeProjectRepository _repository = repository;
	private readonly ILogger<SetSignificantChangeStakeholderConsultationCommandHandler> _logger = logger;

	public async Task<CommandResult> Handle(SetSignificantChangeStakeholderConsultationCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetStakeholderConsultation(
			request.TrustConsultedStakeholders,
			request.TrustConsultedStakeholdersNotConsultedReason);

		_repository.Update(existingProject);
		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}