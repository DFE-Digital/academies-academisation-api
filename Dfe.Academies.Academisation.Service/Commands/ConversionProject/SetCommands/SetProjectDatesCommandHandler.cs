using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetProjectDatesCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetProjectDatesCommandHandler> logger) : IRequestHandler<SetProjectDatesCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository = conversionProjectRepository;
		private readonly ILogger<SetProjectDatesCommandHandler> _logger = logger;

		public async Task<CommandResult> Handle(SetProjectDatesCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id, cancellationToken);

			if (existingProject is null)
			{
				_logger.LogError("Conversion project not found with id: {ProjectId}", request.Id);
				return new NotFoundCommandResult();
			}

			// Update the project dates in the existing project
			existingProject.SetProjectDates(
				request.AdvisoryBoardDate,
				request.PreviousAdvisoryBoard,
				request.ProposedConversionDate,
				request.ProjectDatesSectionComplete,
				request.ReasonsChanged,
				request.ChangedBy);

			_conversionProjectRepository.Update(existingProject as Project);
			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}

