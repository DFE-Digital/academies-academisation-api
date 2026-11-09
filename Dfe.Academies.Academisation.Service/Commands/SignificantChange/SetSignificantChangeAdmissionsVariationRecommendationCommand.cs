using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeAdmissionsVariationRecommendationPublicCommand(
		AdmissionsVariationRecommendationAnswer RecommendationAnswer);

	public record SetSignificantChangeAdmissionsVariationRecommendationCommand(
		int Id,
		AdmissionsVariationRecommendationAnswer RecommendationAnswer)
		: SetSignificantChangeAdmissionsVariationRecommendationPublicCommand(RecommendationAnswer),
			IRequest<CommandResult>;

}
