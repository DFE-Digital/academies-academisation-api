using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

public class SetFormAMatProjectReferenceCommandHandler : IRequestHandler<SetFormAMatProjectReferenceCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly ILogger<SetFormAMatProjectReferenceCommandHandler> _logger;

	public SetFormAMatProjectReferenceCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetFormAMatProjectReferenceCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
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

		existingProject.SetFormAMatProjectReference(request.FormAMatProjectId);

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}

