using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;

public class SetIncomingTrustCommandHandler : IRequestHandler<SetIncomingTrustCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly ILogger<SetIncomingTrustCommandHandler> _logger;

	public SetIncomingTrustCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetIncomingTrustCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetIncomingTrustCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id);

		if (existingProject is null)
		{
			_logger.LogError($"conversion project not found with id:{request.Id}");
			return new NotFoundCommandResult();
		}

		existingProject.SetIncomingTrust(request.TrustReferrenceNumber, request.TrustName);

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
