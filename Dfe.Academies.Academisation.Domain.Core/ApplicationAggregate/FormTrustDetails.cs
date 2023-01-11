using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public class FormTrustDetails
	{
		public FormTrustDetails(DateTime? FormTrustOpeningDate,
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
			string? FormTrustImprovementApprovedSponsor = null)
		{
			this.FormTrustOpeningDate = FormTrustOpeningDate;
			this.FormTrustProposedNameOfTrust = FormTrustProposedNameOfTrust;
			this.TrustApproverName = TrustApproverName;
			this.TrustApproverEmail = TrustApproverEmail;
			this.FormTrustReasonApprovaltoConvertasSAT = FormTrustReasonApprovaltoConvertasSAT;
			this.FormTrustReasonApprovedPerson = FormTrustReasonApprovedPerson;
			this.FormTrustReasonForming = FormTrustReasonForming;
			this.FormTrustReasonVision = FormTrustReasonVision;
			this.FormTrustReasonGeoAreas = FormTrustReasonGeoAreas;
			this.FormTrustReasonFreedom = FormTrustReasonFreedom;
			this.FormTrustReasonImproveTeaching = FormTrustReasonImproveTeaching;
			this.FormTrustPlanForGrowth = FormTrustPlanForGrowth;
			this.FormTrustPlansForNoGrowth = FormTrustPlansForNoGrowth;
			this.FormTrustGrowthPlansYesNo = FormTrustGrowthPlansYesNo;
			this.FormTrustImprovementSupport = FormTrustImprovementSupport;
			this.FormTrustImprovementStrategy = FormTrustImprovementStrategy;
			this.FormTrustImprovementApprovedSponsor = FormTrustImprovementApprovedSponsor;
		}

		public DateTime? FormTrustOpeningDate { get; init; }
		public string? FormTrustProposedNameOfTrust { get; init; }
		public string? TrustApproverName { get; init; }
		public string? TrustApproverEmail { get; init; }
		public bool? FormTrustReasonApprovaltoConvertasSAT { get; init; }
		public string? FormTrustReasonApprovedPerson { get; init; }
		public string? FormTrustReasonForming { get; init; }
		public string? FormTrustReasonVision { get; init; }
		public string? FormTrustReasonGeoAreas { get; init; }
		public string? FormTrustReasonFreedom { get; init; }
		public string? FormTrustReasonImproveTeaching { get; init; }
		public string? FormTrustPlanForGrowth { get; init; }
		public string? FormTrustPlansForNoGrowth { get; init; }
		public bool? FormTrustGrowthPlansYesNo { get; init; }
		public string? FormTrustImprovementSupport { get; init; }
		public string? FormTrustImprovementStrategy { get; init; }
		public string? FormTrustImprovementApprovedSponsor { get; init; }

		public void Deconstruct(out DateTime? FormTrustOpeningDate, out string? FormTrustProposedNameOfTrust, out string? TrustApproverName, out string? TrustApproverEmail, out bool? FormTrustReasonApprovaltoConvertasSAT, out string? FormTrustReasonApprovedPerson, out string? FormTrustReasonForming, out string? FormTrustReasonVision, out string? FormTrustReasonGeoAreas, out string? FormTrustReasonFreedom, out string? FormTrustReasonImproveTeaching, out string? FormTrustPlanForGrowth, out string? FormTrustPlansForNoGrowth, out bool? FormTrustGrowthPlansYesNo, out string? FormTrustImprovementSupport, out string? FormTrustImprovementStrategy, out string? FormTrustImprovementApprovedSponsor)
		{
			FormTrustOpeningDate = this.FormTrustOpeningDate;
			FormTrustProposedNameOfTrust = this.FormTrustProposedNameOfTrust;
			TrustApproverName = this.TrustApproverName;
			TrustApproverEmail = this.TrustApproverEmail;
			FormTrustReasonApprovaltoConvertasSAT = this.FormTrustReasonApprovaltoConvertasSAT;
			FormTrustReasonApprovedPerson = this.FormTrustReasonApprovedPerson;
			FormTrustReasonForming = this.FormTrustReasonForming;
			FormTrustReasonVision = this.FormTrustReasonVision;
			FormTrustReasonGeoAreas = this.FormTrustReasonGeoAreas;
			FormTrustReasonFreedom = this.FormTrustReasonFreedom;
			FormTrustReasonImproveTeaching = this.FormTrustReasonImproveTeaching;
			FormTrustPlanForGrowth = this.FormTrustPlanForGrowth;
			FormTrustPlansForNoGrowth = this.FormTrustPlansForNoGrowth;
			FormTrustGrowthPlansYesNo = this.FormTrustGrowthPlansYesNo;
			FormTrustImprovementSupport = this.FormTrustImprovementSupport;
			FormTrustImprovementStrategy = this.FormTrustImprovementStrategy;
			FormTrustImprovementApprovedSponsor = this.FormTrustImprovementApprovedSponsor;
		}
	}
}
