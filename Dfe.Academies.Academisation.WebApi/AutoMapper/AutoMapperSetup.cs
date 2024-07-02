using Ardalis.GuardClauses;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.AutoMapper;

public static class AutoMapperSetup
{
	public static void AddMappings(Profile profile)
	{
		Guard.Against.NullOrEmpty(nameof(profile));

		// Trust mappings
		profile.CreateMap<IJoinTrust, ApplicationJoinTrustServiceModel>();
		profile.CreateMap<NewProjectServiceModel, NewProject>();
		profile.CreateMap<NewProjectTrustServiceModel, NewProjectTrust>();
		profile.CreateMap<NewProjectSchoolServiceModel, NewProjectSchool>();

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

		profile.CreateMap<TrustKeyPersonServiceModel, TrustKeyPerson>()
			.ForMember(x => x.DynamicsKeyPersonId, opt => opt.Ignore())
			.ForMember(x => x.CreatedOn, opt => opt.Ignore())
			.ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
			.ForMember(x => x.DomainEvents, opt => opt.Ignore()) // Ignore DomainEvents property
			.ReverseMap();

		profile.CreateMap<TrustKeyPersonRoleServiceModel, TrustKeyPersonRole>()
			.ForMember(x => x.CreatedOn, opt => opt.Ignore())
			.ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
			.ForMember(x => x.DomainEvents, opt => opt.Ignore()) // Ignore DomainEvents property
			.ReverseMap();

		profile.CreateMap<TrustKeyPersonRoleServiceModel, ITrustKeyPersonRole>().ReverseMap();

		profile.CreateMap<IApplication, ApplicationSchoolSharepointServiceModel>()
			.ConvertUsing((wrapper, destination, context) =>
			new ApplicationSchoolSharepointServiceModel(wrapper.Id, wrapper.ApplicationReference!, wrapper.Schools.Select(x => new SchoolSharepointServiceModel(x.Id, x.Details.SchoolName, x.EntityId)).ToList()));
	}
}
