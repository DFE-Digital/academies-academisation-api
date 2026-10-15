using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeAdmissionsVariationRecommendationPublicCommand(
		AdmissionsVariationRecommendationAnswer RecommendationAnswer,
		string? FurtherInformation);

	public record SetSignificantChangeAdmissionsVariationRecommendationCommand(
		int Id,
		AdmissionsVariationRecommendationAnswer RecommendationAnswer,
		string? FurtherInformation)
		: SetSignificantChangeAdmissionsVariationRecommendationPublicCommand(RecommendationAnswer, FurtherInformation),
			IRequest<CommandResult>;

}
