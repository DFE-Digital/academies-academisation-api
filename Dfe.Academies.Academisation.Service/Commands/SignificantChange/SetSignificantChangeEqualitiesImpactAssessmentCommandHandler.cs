using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public class SetSignificantChangeEqualitiesImpactAssessmentCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeEqualitiesImpactAssessmentCommandHandler> logger) : IRequestHandler<SetSignificantChangeEqualitiesImpactAssessmentCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetSignificantChangeEqualitiesImpactAssessmentCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

			if (existingProject is null)
			{
				logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
				return new NotFoundCommandResult();
			}

			existingProject.SetEqualitiesImpactAssessment(
				request.EqualitiesImpactAssessmentCompleted,
				request.EqualitiesImpactIdentified,
				request.EqualitiesImpactIdentifiedMitigation);

			repository.Update(existingProject);
			await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}
