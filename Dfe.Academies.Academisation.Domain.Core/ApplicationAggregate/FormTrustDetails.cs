using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record FormTrustDetails(
		DateTime? FormTrustOpeningDate = null,
		string? FormTrustProposedNameOfTrust = null,
		string? TrustApproverName = null,
		string? TrustApproverEmail = null,
		bool? FormTrustReasonApprovaltoConvertasSAT = null,
		string? FormTrustReasonApprovedPerson = null,
		string? FormTrustReasonForming = null,
		string? FormTrustReasonVision = null,
		string? FormTrustReasonGeoAreas = null,
		string? FormTrustReasonFreedom = null,
		string? FormTrustReasonImproveTeaching = null,
		string? FormTrustPlanForGrowth = null,
		string? FormTrustPlansForNoGrowth = null,
		bool? FormTrustGrowthPlansYesNo = null,
		string? FormTrustImprovementSupport = null,
		string? FormTrustImprovementStrategy = null,
		string? FormTrustImprovementApprovedSponsor = null);

	
}
