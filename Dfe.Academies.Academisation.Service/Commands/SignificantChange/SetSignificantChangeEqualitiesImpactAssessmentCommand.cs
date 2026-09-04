using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
    public record SetSignificantChangeEqualitiesImpactAssessmentPublicCommand(
        bool? EqualitiesImpactAssessmentCompleted,
        EqualitiesImpact? EqualitiesImpactIdentified,
        string? EqualitiesImpactIdentifiedMitigation);


	public record SetSignificantChangeEqualitiesImpactAssessmentCommand(
		int Id,
		bool? EqualitiesImpactAssessmentCompleted,
        EqualitiesImpact? EqualitiesImpactIdentified,
        string? EqualitiesImpactIdentifiedMitigation): IRequest<CommandResult>;
}
