using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetConversionPublicEqualityDutyCommandHandler : IRequestHandler<SetConversionPublicEqualityDutyCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly ILogger<SetConversionPublicEqualityDutyCommandHandler> _logger;

		public SetConversionPublicEqualityDutyCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetConversionPublicEqualityDutyCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetConversionPublicEqualityDutyCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id, cancellationToken);

			if (existingProject != null)
			{
				existingProject.SetPublicEqualityDuty(request.PublicEqualityDutyImpact, request.PublicEqualityDutyReduceImpactReason, request.PublicEqualityDutySectionComplete);
				_conversionProjectRepository.Update((Project)existingProject);

				await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
			else
			{
				_logger.LogError("conversion project not found with id:{Id}", request.Id);
				return new NotFoundCommandResult();
			}

			return new CommandSuccessResult();
		}
	}
}
