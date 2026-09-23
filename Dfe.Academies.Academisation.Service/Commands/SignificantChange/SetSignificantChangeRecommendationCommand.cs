using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeRecommendationPublicCommand(
        Decision? Recommendation,
        string? RecommendationMoreInformation);

	public record SetSignificantChangeRecommendationCommand(
		int Id,
        Decision? Recommendation,
        string? RecommendationMoreInformation)
		: SetSignificantChangeRecommendationPublicCommand(Recommendation,
			RecommendationMoreInformation), IRequest<CommandResult>;
}
