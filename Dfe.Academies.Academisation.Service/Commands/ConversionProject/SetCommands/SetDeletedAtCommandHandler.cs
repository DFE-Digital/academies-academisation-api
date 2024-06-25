using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

public class SetDeletedAtCommandHandler : IRequestHandler<SetDeletedAtCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly ILogger<SetDeletedAtCommandHandler> _logger;

	public SetDeletedAtCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetDeletedAtCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetDeletedAtCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.ProjectId, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError($"Conversion project not found with id: {request.ProjectId}");
			return new NotFoundCommandResult();
		}

		existingProject.SetDeletedAt();

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}

