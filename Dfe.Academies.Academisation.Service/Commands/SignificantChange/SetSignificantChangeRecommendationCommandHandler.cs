using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeRecommendationCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeRecommendationCommandHandler> logger) : IRequestHandler<SetSignificantChangeRecommendationCommand, CommandResult>
{
	public async Task<CommandResult> Handle(SetSignificantChangeRecommendationCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetRecommendation(
			request.Recommendation,
			request.RecommendationMoreInformation);

		repository.Update(existingProject);
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}