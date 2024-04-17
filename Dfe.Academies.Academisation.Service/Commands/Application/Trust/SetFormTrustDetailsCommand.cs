using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record SetFormTrustDetailsCommand(
		int applicationId,
		DateTime? FormTrustOpeningDate,
		string? FormTrustProposedNameOfTrust,
		string? TrustApproverName,
		string? TrustApproverEmail,
		bool? FormTrustReasonApprovaltoConvertasSAT,
		string? FormTrustReasonApprovedPerson,
		string? FormTrustReasonForming,
		string? FormTrustReasonVision,
		string? FormTrustReasonGeoAreas,
		string? FormTrustReasonFreedom,
		string? FormTrustReasonImproveTeaching,
		string? FormTrustPlanForGrowth,
		string? FormTrustPlansForNoGrowth,
		bool? FormTrustGrowthPlansYesNo,
		string? FormTrustImprovementSupport,
		string? FormTrustImprovementStrategy,
		string? FormTrustImprovementApprovedSponsor) : IRequest<CommandResult>;
}
