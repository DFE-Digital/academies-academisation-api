using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.WebApi.AutoMapper;

namespace Dfe.Academies.Academisation.Service.AutoMapper;

public static class AutoMapperSetup
{
	public static void AddMappings(Profile profile)
	{

		Guard.Against.NullOrEmpty(nameof(profile));

		// Trust mappings
		profile.CreateMap<IJoinTrust, JoinTrustState>()
			.ForMember(x => x.CreatedOn, opt => opt.Ignore())
			.ForMember(x => x.LastModifiedOn, opt => opt.Ignore());

		profile.CreateMap<JoinTrustState, JoinTrust>();
		profile.CreateMap<IJoinTrust, ApplicationJoinTrustServiceModel>();

		profile.CreateMap<IFormTrust, FormTrustState>()
			.ForMember(x => x.CreatedOn, opt => opt.Ignore())
			.ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
			.ForMember(x => x.FormTrustImprovementSupport, opt => opt.MapFrom(s => s.TrustDetails.FormTrustImprovementSupport))
			.ForMember(x => x.FormTrustImprovementApprovedSponsor, opt => opt.MapFrom(s => s.TrustDetails.FormTrustImprovementApprovedSponsor))
			.ForMember(x => x.FormTrustProposedNameOfTrust, opt => opt.MapFrom(s => s.TrustDetails.FormTrustProposedNameOfTrust))
			.ForMember(x => x.TrustApproverName, opt => opt.MapFrom(s => s.TrustDetails.TrustApproverName))
			.ForMember(x => x.FormTrustGrowthPlansYesNo, opt => opt.MapFrom(s => s.TrustDetails.FormTrustGrowthPlansYesNo))
			.ForMember(x => x.FormTrustImprovementApprovedSponsor, opt => opt.MapFrom(s => s.TrustDetails.FormTrustImprovementApprovedSponsor))
			.ForMember(x => x.FormTrustImprovementStrategy, opt => opt.MapFrom(s => s.TrustDetails.FormTrustImprovementStrategy))
			.ForMember(x => x.FormTrustOpeningDate, opt => opt.MapFrom(s => s.TrustDetails.FormTrustOpeningDate))
			.ForMember(x => x.FormTrustPlanForGrowth, opt => opt.MapFrom(s => s.TrustDetails.FormTrustPlanForGrowth))
			.ForMember(x => x.FormTrustPlansForNoGrowth, opt => opt.MapFrom(s => s.TrustDetails.FormTrustPlansForNoGrowth))
			.ForMember(x => x.FormTrustProposedNameOfTrust, opt => opt.MapFrom(s => s.TrustDetails.FormTrustProposedNameOfTrust))
			.ForMember(x => x.FormTrustReasonApprovaltoConvertasSAT, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonApprovaltoConvertasSAT))
			.ForMember(x => x.FormTrustReasonApprovedPerson, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonApprovedPerson))
			.ForMember(x => x.FormTrustReasonForming, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonForming))
			.ForMember(x => x.FormTrustReasonFreedom, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonFreedom))
			.ForMember(x => x.FormTrustReasonGeoAreas, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonGeoAreas))
			.ForMember(x => x.FormTrustReasonImproveTeaching, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonImproveTeaching))
			.ForMember(x => x.FormTrustReasonVision, opt => opt.MapFrom(s => s.TrustDetails.FormTrustReasonVision))
			.ForMember(x => x.TrustApproverEmail, opt => opt.MapFrom(s => s.TrustDetails.TrustApproverEmail))
			.ForMember(x => x.TrustApproverName, opt => opt.MapFrom(s => s.TrustDetails.TrustApproverName));

		profile.CreateMap<FormTrustState, FormTrust>()
			.ForPath(x => x.TrustDetails.FormTrustOpeningDate, opt => opt.MapFrom(src => src.FormTrustOpeningDate))
			.ForPath(x => x.TrustDetails.FormTrustImprovementSupport, opt => opt.MapFrom(src => src.FormTrustImprovementSupport))
			.ForPath(x => x.TrustDetails.FormTrustImprovementApprovedSponsor, opt => opt.MapFrom(src => src.FormTrustImprovementApprovedSponsor))
			.ForPath(x => x.TrustDetails.FormTrustProposedNameOfTrust, opt => opt.MapFrom(src => src.FormTrustProposedNameOfTrust))
			.ForPath(x => x.TrustDetails.TrustApproverName, opt => opt.MapFrom(src => src.TrustApproverName))
			.ForPath(x => x.TrustDetails.FormTrustGrowthPlansYesNo, opt => opt.MapFrom(src => src.FormTrustGrowthPlansYesNo))
			.ForPath(x => x.TrustDetails.FormTrustImprovementApprovedSponsor, opt => opt.MapFrom(src => src.FormTrustImprovementApprovedSponsor))
			.ForPath(x => x.TrustDetails.FormTrustImprovementStrategy, opt => opt.MapFrom(src => src.FormTrustImprovementStrategy))
			.ForPath(x => x.TrustDetails.FormTrustOpeningDate, opt => opt.MapFrom(src => src.FormTrustOpeningDate))
			.ForPath(x => x.TrustDetails.FormTrustPlanForGrowth, opt => opt.MapFrom(src => src.FormTrustPlanForGrowth))
			.ForPath(x => x.TrustDetails.FormTrustPlansForNoGrowth, opt => opt.MapFrom(src => src.FormTrustPlansForNoGrowth))
			.ForPath(x => x.TrustDetails.FormTrustProposedNameOfTrust, opt => opt.MapFrom(src => src.FormTrustProposedNameOfTrust))
			.ForPath(x => x.TrustDetails.FormTrustReasonApprovaltoConvertasSAT, opt => opt.MapFrom(src => src.FormTrustReasonApprovaltoConvertasSAT))
			.ForPath(x => x.TrustDetails.FormTrustReasonApprovedPerson, opt => opt.MapFrom(src => src.FormTrustReasonApprovedPerson))
			.ForPath(x => x.TrustDetails.FormTrustReasonForming, opt => opt.MapFrom(src => src.FormTrustReasonForming))
			.ForPath(x => x.TrustDetails.FormTrustReasonFreedom, opt => opt.MapFrom(src => src.FormTrustReasonFreedom))
			.ForPath(x => x.TrustDetails.FormTrustReasonGeoAreas, opt => opt.MapFrom(src => src.FormTrustReasonGeoAreas))
			.ForPath(x => x.TrustDetails.FormTrustReasonImproveTeaching, opt => opt.MapFrom(src => src.FormTrustReasonImproveTeaching))
			.ForPath(x => x.TrustDetails.FormTrustReasonVision, opt => opt.MapFrom(src => src.FormTrustReasonVision))
			.ForPath(x => x.TrustDetails.TrustApproverEmail, opt => opt.MapFrom(src => src.TrustApproverEmail))
			.ForPath(x => x.TrustDetails.TrustApproverName, opt => opt.MapFrom(src => src.TrustApproverName));

		//profile.CreateMap<FormTrustState, FormTrust>()
		//	.ForMember(x => x.TrustDetails, opt => opt.Ignore());
		//profile.CreateMap<IFormTrust, ApplicationFormTrustServiceModel>();

	}
}
