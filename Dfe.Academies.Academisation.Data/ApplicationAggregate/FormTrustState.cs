using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationFormTrust")]
	public class FormTrustState : BaseEntity
	{
		public DateTime? FormTrustOpeningDate { get; set; }
		public string? FormTrustProposedNameOfTrust { get; set; }
		public string? TrustApproverName { get; set; }
		public string? TrustApproverEmail { get; set; }
		public bool? FormTrustReasonApprovaltoConvertasSAT { get; set; }
		public string? FormTrustReasonApprovedPerson { get; set; }
		public string? FormTrustReasonForming { get; set; }
		public string? FormTrustReasonVision { get; set; }
		public string? FormTrustReasonGeoAreas { get; set; }
		public string? FormTrustReasonFreedom { get; set; }
		public string? FormTrustReasonImproveTeaching { get; set; }
		public string? FormTrustPlanForGrowth { get; set; }
		public string? FormTrustPlansForNoGrowth { get; set; }
		public bool? FormTrustGrowthPlansYesNo { get; set; }
		public string? FormTrustImprovementSupport { get; set; }
		public string? FormTrustImprovementStrategy { get; set; }
		public string? FormTrustImprovementApprovedSponsor { get; set; }
	}
}
