using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeRecommendationPublicCommand(
        Recommendation? Recommendation,
        string? RecommendationMoreInformation);

	public record SetSignificantChangeRecommendationCommand(
		int Id,
        Recommendation? Recommendation,
        string? RecommendationMoreInformation)
		: SetSignificantChangeRecommendationPublicCommand(Recommendation,
			RecommendationMoreInformation), IRequest<CommandResult>;
}
