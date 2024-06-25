using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;

public class SetExternalApplicationFormCommandHandler : IRequestHandler<SetExternalApplicationFormCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly ILogger<SetExternalApplicationFormCommandHandler> _logger;

	public SetExternalApplicationFormCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetExternalApplicationFormCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetExternalApplicationFormCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError($"conversion project not found with id:{request.Id}");
			return new NotFoundCommandResult();
		}

		existingProject.SetExternalApplicationForm(request.ExternalApplicationFormSaved, request.ExternalApplicationFormUrl);

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
