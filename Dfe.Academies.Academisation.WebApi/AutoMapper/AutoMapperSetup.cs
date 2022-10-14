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

		// the mapping for this object is awkward because of the use of records, may have to re-think someof this but this is the best for now
		// also leaving the commented out code at the bottom in for reference
		profile.CreateMap<FormTrustState, FormTrustDetails>().ReverseMap();

		profile.CreateMap<FormTrustState, IFormTrust>()
			.ForMember(dest => dest.TrustDetails, opt => opt.MapFrom(src => src))
			.ReverseMap();

		profile.CreateMap<FormTrustState, FormTrust>()
			.ForMember(dest => dest.TrustDetails, opt => opt.MapFrom(src => src))
			.ForCtorParam(nameof(FormTrust.TrustDetails), opt => opt.MapFrom(src => src))
			.ReverseMap();

		profile.CreateMap<IFormTrust, ApplicationFormTrustServiceModel>()
			.ConvertUsing((wrapper, destination, context) => new ApplicationFormTrustServiceModel(
				wrapper.Id,
				wrapper.TrustDetails.FormTrustOpeningDate,
				wrapper.TrustDetails.FormTrustProposedNameOfTrust,
				wrapper.TrustDetails.TrustApproverName,
				wrapper.TrustDetails.TrustApproverEmail,
				wrapper.TrustDetails.FormTrustReasonApprovaltoConvertasSAT,
				wrapper.TrustDetails.FormTrustReasonApprovedPerson,
				wrapper.TrustDetails.FormTrustReasonForming,
				wrapper.TrustDetails.FormTrustReasonVision,
				wrapper.TrustDetails.FormTrustReasonGeoAreas,
				wrapper.TrustDetails.FormTrustReasonFreedom,
				wrapper.TrustDetails.FormTrustReasonImproveTeaching,
				wrapper.TrustDetails.FormTrustPlanForGrowth,
				wrapper.TrustDetails.FormTrustPlansForNoGrowth,
				wrapper.TrustDetails.FormTrustGrowthPlansYesNo,
				wrapper.TrustDetails.FormTrustImprovementSupport,
				wrapper.TrustDetails.FormTrustImprovementStrategy,
				wrapper.TrustDetails.FormTrustImprovementApprovedSponsor));
		
		//.MapRecordMember(x => x.FormTrustOpeningDate, src => src.TrustDetails.FormTrustOpeningDate)
		//.MapRecordMember(x => x.FormTrustImprovementSupport, src => src.TrustDetails.FormTrustImprovementSupport)
		//.MapRecordMember(x => x.FormTrustImprovementApprovedSponsor, src => src.TrustDetails.FormTrustImprovementApprovedSponsor)
		//.MapRecordMember(x => x.FormTrustProposedNameOfTrust, src => src.TrustDetails.FormTrustProposedNameOfTrust)
		//.MapRecordMember(x => x.TrustApproverName, src => src.TrustDetails.TrustApproverName)
		//.MapRecordMember(x => x.FormTrustGrowthPlansYesNo, src => src.TrustDetails.FormTrustGrowthPlansYesNo)
		//.MapRecordMember(x => x.FormTrustImprovementApprovedSponsor, src => src.TrustDetails.FormTrustImprovementApprovedSponsor)
		//.MapRecordMember(x => x.FormTrustImprovementStrategy, src => src.TrustDetails.FormTrustImprovementStrategy)
		//.MapRecordMember(x => x.FormTrustOpeningDate, src => src.TrustDetails.FormTrustOpeningDate)
		//.MapRecordMember(x => x.FormTrustPlanForGrowth, src => src.TrustDetails.FormTrustPlanForGrowth)
		//.MapRecordMember(x => x.FormTrustPlansForNoGrowth, src => src.TrustDetails.FormTrustPlansForNoGrowth)
		//.MapRecordMember(x => x.FormTrustProposedNameOfTrust, src => src.TrustDetails.FormTrustProposedNameOfTrust)
		//.MapRecordMember(x => x.FormTrustReasonApprovaltoConvertasSAT, src => src.TrustDetails.FormTrustReasonApprovaltoConvertasSAT)
		//.MapRecordMember(x => x.FormTrustReasonApprovedPerson, src => src.TrustDetails.FormTrustReasonApprovedPerson)
		//.MapRecordMember(x => x.FormTrustReasonForming, src => src.TrustDetails.FormTrustReasonForming)
		//.MapRecordMember(x => x.FormTrustReasonFreedom, src => src.TrustDetails.FormTrustReasonFreedom)
		//.MapRecordMember(x => x.FormTrustReasonGeoAreas, src => src.TrustDetails.FormTrustReasonGeoAreas)
		//.MapRecordMember(x => x.FormTrustReasonImproveTeaching, src => src.TrustDetails.FormTrustReasonImproveTeaching)
		//.MapRecordMember(x => x.FormTrustReasonVision, src => src.TrustDetails.FormTrustReasonVision)
		//.MapRecordMember(x => x.TrustApproverEmail, src => src.TrustDetails.TrustApproverEmail)
		//.MapRecordMember(x => x.TrustApproverName, src => src.TrustDetails.TrustApproverName);

	}
}
