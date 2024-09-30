using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateCompleteProjectsCommandHandler : IRequestHandler<CreateCompleteProjectsCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILogger<CreateCompleteProjectsCommandHandler> _logger;

		public CreateCompleteProjectsCommandHandler(
			IConversionProjectRepository conversionProjectRepository,
			IDateTimeProvider dateTimeProvider,
			ILogger<CreateCompleteProjectsCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_dateTimeProvider = dateTimeProvider;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateCompleteProjectsCommand request,
			CancellationToken cancellationToken)
		{
			// find prjects with approved decisions that have not been sent - use returned complete project Id

			// for each project send to complete

			// we will need a http client with a connection to complete so appsettings changes

			var conversionProjects = await _conversionProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				_logger.LogInformation($"No conversion projects found which require form a mat projects creating.");
				return new NotFoundCommandResult();
			}

			foreach (var conversionProject in conversionProjects)
			{
				// populate the complete dto 

				// send to complete, get the response and set the CompleteProjectId
				_conversionProjectRepository.Update(conversionProject as Domain.ProjectAggregate.Project);
			}


			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);


			// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
