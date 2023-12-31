using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class SetSchoolOverviewCommandHandler : IRequestHandler<SetSchoolOverviewCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly ILogger<SetSchoolOverviewCommandHandler> _logger;

		public SetSchoolOverviewCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetSchoolOverviewCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetSchoolOverviewCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id);

			if (existingProject is null)
			{
				_logger.LogError($"Conversion project not found with id: {request.Id}");
				return new NotFoundCommandResult();
			}

			// Update the school overview information in the existing project
			existingProject.SetSchoolOverview(
				request.PublishedAdmissionNumber,
				request.ViabilityIssues,
				request.PartOfPfiScheme,
				request.FinancialDeficit,
				request.NumberOfPlacesFundedFor,
				request.PfiSchemeDetails,
				request.DistanceFromSchoolToTrustHeadquarters,
				request.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				request.MemberOfParliamentNameAndParty);

			_conversionProjectRepository.Update(existingProject as Project);
			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}

