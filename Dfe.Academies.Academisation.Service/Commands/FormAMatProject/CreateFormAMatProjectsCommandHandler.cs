using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class CreateFormAMatProjectsCommandHandler : IRequestHandler<CreateFormAMatProjectsCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IFormAMatProjectRepository _formAMatProjectRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILogger<CreateFormAMatProjectsCommandHandler> _logger;

		public CreateFormAMatProjectsCommandHandler(IConversionProjectRepository conversionProjectRepository, IFormAMatProjectRepository formAMatProjectRepository, IDateTimeProvider dateTimeProvider,
			ILogger<CreateFormAMatProjectsCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_formAMatProjectRepository = formAMatProjectRepository;
			_dateTimeProvider = dateTimeProvider;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateFormAMatProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var conversionProjects = await _conversionProjectRepository.GetConversionProjectsThatRequireFormAMatCreation(cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				_logger.LogInformation($"No conversion projects found which require form a mat projects creating.");
				return new NotFoundCommandResult();
			}

			foreach (var conversionProject in conversionProjects)
			{
				var formAMat = await _formAMatProjectRepository.GetByApplicationReference(conversionProject.Details.ApplicationReferenceNumber, cancellationToken).ConfigureAwait(false);
				if (formAMat == null) {
					// create formAMat
					formAMat = FormAMatProject.Create(conversionProject.Details.NameOfTrust, conversionProject.Details.ApplicationReferenceNumber, _dateTimeProvider.Now);
					_formAMatProjectRepository.Insert(formAMat);
					await _formAMatProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
				}

				conversionProject.SetFormAMatProjectId(formAMat.Id);

				_conversionProjectRepository.Update(conversionProject as Domain.ProjectAggregate.Project);
			}

			
			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			

			// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
