using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;

public class SetPerformanceDataCommandHandler : IRequestHandler<SetPerformanceDataCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly ILogger<SetPerformanceDataCommandHandler> _logger;

	public SetPerformanceDataCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetPerformanceDataCommandHandler> logger)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetPerformanceDataCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id);

		if (existingProject is null)
		{
			_logger.LogError($"conversion project not found with id:{request.Id}");
			return new NotFoundCommandResult();
		}

		existingProject.SetPerformanceData(request.KeyStage2PerformanceAdditionalInformation, request.KeyStage4PerformanceAdditionalInformation, request.KeyStage5PerformanceAdditionalInformation, request.EducationalAttendanceAdditionalInformation);

		_conversionProjectRepository.Update(existingProject as Project);
		await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
