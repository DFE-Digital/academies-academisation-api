using Ardalis.GuardClauses;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.AutoMapper;

public static class AutoMapperSetup
{
	public static void AddMappings(Profile profile)
	{
		Guard.Against.NullOrEmpty(nameof(profile));

		// Trust mappings
		//profile.CreateMap<IJoinTrust, JoinTrustState>()
		//	.ForMember(x => x.CreatedOn, opt => opt.Ignore())
		//	.ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
		//	.ForMember(x => x.DynamicsApplicationId, opt => opt.Ignore());
		//profile.CreateMap<JoinTrustState, JoinTrust>();
		profile.CreateMap<IJoinTrust, ApplicationJoinTrustServiceModel>();

		// the mapping for this object is awkward because of the use of records, may have to re-think someof this but this is the best for now
		//profile.CreateMap<FormTrustState, FormTrustDetails>().ReverseMap();

		//profile.CreateMap<FormTrustState, IFormTrust>()
		//	.ForMember(dest => dest.TrustDetails, opt => opt.MapFrom(src => src))
		//	.ForMember(dest => dest.KeyPeople, opt => opt.Ignore())
		//	.ReverseMap().ForMember(x => x.KeyPeople, opt => opt.Ignore());

		//profile.CreateMap<FormTrustState, FormTrust>()
		//	.ForMember(dest => dest.TrustDetails, opt => opt.MapFrom(src => src))
		//	.ForCtorParam(nameof(FormTrust.TrustDetails), opt => opt.MapFrom(src => src))
		//	.ForCtorParam(nameof(FormTrust.KeyPeople), (opt) => opt.MapFrom(x => x.KeyPeople))
		//	.ReverseMap();//.ForMember(x => x.KeyPeople, opt => opt.Ignore());

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
				wrapper.TrustDetails.FormTrustImprovementApprovedSponsor,
				wrapper.KeyPeople.Select(x => new TrustKeyPersonServiceModel(x.Id, x.Name, x.DateOfBirth, x.Biography, context.Mapper.Map<IEnumerable<TrustKeyPersonRoleServiceModel>>(x.Roles))).ToList()));

		//profile.CreateMap<TrustKeyPersonState, ITrustKeyPerson>().ReverseMap();
		//profile.CreateMap<TrustKeyPersonState, TrustKeyPerson>()
		//	.ForCtorParam(nameof(TrustKeyPerson.Roles), (opt) => opt.MapFrom(x => x.Roles))
		//	.ReverseMap();

		profile.CreateMap<TrustKeyPersonServiceModel, TrustKeyPerson>().ReverseMap();

		//profile.CreateMap<TrustKeyPersonRoleState, ITrustKeyPersonRole>().ReverseMap();
		//profile.CreateMap<TrustKeyPersonRoleState, TrustKeyPersonRole>().ReverseMap();
		profile.CreateMap<TrustKeyPersonRoleServiceModel, TrustKeyPersonRole>().ReverseMap();
		profile.CreateMap<TrustKeyPersonRoleServiceModel, ITrustKeyPersonRole>().ReverseMap();
	}
}
