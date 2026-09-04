using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeAdmissionVariationConsultationPublicCommand(
	bool? consultationIncludeAdmissionVariation,
	string? noAdmissionVariationReason) : IRequest<CommandResult>
{
	public bool? ConsultationIncludeAdmissionVariation { get; set; } = consultationIncludeAdmissionVariation;
	public string? NoAdmissionVariationReason { get; set; } = noAdmissionVariationReason;
}

public class SetSignificantChangeAdmissionVariationConsultationCommand(
	int id,
	bool? consultationIncludeAdmissionVariation,
	string? noAdmissionVariationReason)
	: SetSignificantChangeAdmissionVariationConsultationPublicCommand(consultationIncludeAdmissionVariation, noAdmissionVariationReason)
{
	public int Id { get; set; } = id;
}
