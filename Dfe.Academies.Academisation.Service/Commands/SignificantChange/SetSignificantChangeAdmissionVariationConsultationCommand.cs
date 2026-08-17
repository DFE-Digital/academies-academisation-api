using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeAdmissionVariationConsultationPublicCommand(
	bool? consultationIncludeAdmissionVariation,
	bool? consultationIncludeAdmissionVariationNotApplicable,
	string? noAdmissionVariationReason) : IRequest<CommandResult>
{
	public bool? ConsultationIncludeAdmissionVariation { get; set; } = consultationIncludeAdmissionVariation;
	public bool? ConsultationIncludeAdmissionVariationNotApplicable { get; set; } = consultationIncludeAdmissionVariationNotApplicable;
	public string? NoAdmissionVariationReason { get; set; } = noAdmissionVariationReason;
}

public class SetSignificantChangeAdmissionVariationConsultationCommand(
	int id,
	bool? consultationIncludeAdmissionVariation,
	bool? consultationIncludeAdmissionVariationNotApplicable,
	string? noAdmissionVariationReason)
	: SetSignificantChangeAdmissionVariationConsultationPublicCommand(consultationIncludeAdmissionVariation, consultationIncludeAdmissionVariationNotApplicable, noAdmissionVariationReason)
{
	public int Id { get; set; } = id;
}
