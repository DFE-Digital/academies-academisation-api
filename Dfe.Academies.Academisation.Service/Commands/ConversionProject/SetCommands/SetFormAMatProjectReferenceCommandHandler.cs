using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

public class SetFormAMatProjectReferenceCommandHandler : IRequestHandler<SetFormAMatProjectReferenceCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly IFormAMatProjectRepository _formAMatProjectRepository;
	private readonly ILogger<SetFormAMatProjectReferenceCommandHandler> _logger;

	public SetFormAMatProjectReferenceCommandHandler(
		IConversionProjectRepository conversionProjectRepository, 
		IFormAMatProjectRepository formAMatProjectRepository,
		ILogger<SetFormAMatProjectReferenceCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_formAMatProjectRepository = formAMatProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetFormAMatProjectReferenceCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.ProjectId, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError($"Conversion project not found with id: {request.ProjectId}");
			return new NotFoundCommandResult();
		}

		var formAMatProject = await _formAMatProjectRepository.GetById(request.FormAMatProjectId);

		existingProject.SetFormAMatProjectReference(request.FormAMatProjectId);
		existingProject.SetIncomingTrust(formAMatProject.TrustReferenceNumber, formAMatProject.ProposedTrustName);

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}

