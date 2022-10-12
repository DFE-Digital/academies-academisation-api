﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	//just one field for now to get the mapping of the objects right
	public record FormTrustDetails(
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
		string?	 FormTrustImprovementSupport,
		string? FormTrustImprovementStrategy,
		string? FormTrustImprovementApprovedSponsor);
}
