using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeAdmissionVariationPublicCommand(
	bool? AdmissionsVariationRequired,
	string? AdmissionsVariationSupportingEvidence,
	bool? AdmissionsVariationLocalAuthoritySupportive,
	bool? AdmissionsVariationPupilPlacePlanningSupportive,
	bool? AdmissionsVariationAdmissionsTeamSupportive
);

public record SetSignificantChangeAdmissionVariationCommand(
		int Id,
		bool? AdmissionsVariationRequired,
		string? AdmissionsVariationSupportingEvidence,
		bool? AdmissionsVariationLocalAuthoritySupportive,
		bool? AdmissionsVariationPupilPlacePlanningSupportive,
		bool? AdmissionsVariationAdmissionsTeamSupportive
	)
	: SetSignificantChangeAdmissionVariationPublicCommand(
		AdmissionsVariationRequired,
		AdmissionsVariationSupportingEvidence,
		AdmissionsVariationLocalAuthoritySupportive,
		AdmissionsVariationPupilPlacePlanningSupportive,
		AdmissionsVariationAdmissionsTeamSupportive
	),
		IRequest<CommandResult>;
