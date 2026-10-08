using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
    public record SetSignificantChangeEqualitiesImpactAssessmentPublicCommand(
        bool? EqualitiesImpactAssessmentCompleted,
        EqualitiesImpact? EqualitiesImpactIdentified,
        string? EqualitiesLikelyDetails,
        string? EqualitiesSomeImpactDetails,
        string? EqualitiesImpactSupportingEvidence);


	public record SetSignificantChangeEqualitiesImpactAssessmentCommand(
		int Id,
		bool? EqualitiesImpactAssessmentCompleted,
        EqualitiesImpact? EqualitiesImpactIdentified,
        string? EqualitiesLikelyDetails,
        string? EqualitiesSomeImpactDetails,
		string? EqualitiesImpactSupportingEvidence) : IRequest<CommandResult>;
}
