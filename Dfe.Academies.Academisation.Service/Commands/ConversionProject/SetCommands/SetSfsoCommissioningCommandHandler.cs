using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetSfsoCommissioningCommandHandler : IRequestHandler<SetSfsoCommissioningCommand, CommandResult>
    {
        private const int MaxOverviewLength = 250;
        private readonly IConversionProjectRepository _conversionProjectRepository;
        private readonly ILogger<SetSfsoCommissioningCommandHandler> _logger;

        public SetSfsoCommissioningCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetSfsoCommissioningCommandHandler> logger)
        {
            _conversionProjectRepository = conversionProjectRepository;
            _logger = logger;
        }

        public async Task<CommandResult> Handle(SetSfsoCommissioningCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.SfsoCommissioningOverview) && request.SfsoCommissioningOverview.Length > MaxOverviewLength)
            {
                return new CommandValidationErrorResult(new List<ValidationError>
                {
                    new("SfsoCommissioningOverview", $"The overview must be {MaxOverviewLength} characters or less")
                });
            }

            var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id, cancellationToken);
            if (existingProject is null)
            {
                _logger.LogError("conversion project not found with id:{Id}", request.Id);
                return new NotFoundCommandResult();
            }

            existingProject.SetSfsoCommissioning(request.SfsoCommissioningOverview);
            _conversionProjectRepository.Update((Project)existingProject);
            await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new CommandSuccessResult();
        }
    }
}