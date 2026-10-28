using AutoMapper;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

namespace Dfe.Academies.Academisation.Service.Mappers.SignificantChange;

public class SignificantChangeProjectMappingProfile : Profile
{
	public SignificantChangeProjectMappingProfile()
	{
		CreateMap<SignificantChangeProject, SignificantChangeProjectDto>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.Status.ToString()))
			.ForMember(destination => destination.TrustConsultedStakeholders,
				options => options.MapFrom(source => source.Details.TrustConsultedStakeholders))
			.ForMember(destination => destination.TrustConsultedStakeholdersNotConsultedReason,
				options => options.MapFrom(source => source.Details.TrustConsultedStakeholdersNotConsultedReason))
			.ForMember(destination => destination.ConsultationLastedMinimumThreeWeeks,
				options => options.MapFrom(source => source.Details.ConsultationLastedMinimumThreeWeeks))
			.ForMember(destination => destination.ConsultationDurationNotMetReason,
				options => options.MapFrom(source => source.Details.ConsultationDurationNotMetReason))
			.ForMember(destination => destination.ConsultationDurationTaskStatus,
				options => options.MapFrom(source => source.Details.GetConsultationDurationTaskStatus().ToString()))
			.ForMember(destination => destination.FundingAnswer,
				options => options.MapFrom(source => source.Details.FundingAnswer))
			.ForMember(destination => destination.FundingAdditionalInformation,
				options => options.MapFrom(source => source.Details.FundingAdditionalInformation))
			.ForMember(destination => destination.FundingSupportingEvidence,
				options => options.MapFrom(source => source.Details.FundingSupportingEvidence))
			.ForMember(destination => destination.FundingTaskStatus,
				options => options.MapFrom(source => source.Details.GetFundingTaskStatus().ToString()))
			.ForMember(destination => destination.HqtfConsiderationAnswer,
				options => options.MapFrom(source => source.Details.HqtfConsiderationAnswer))
			.ForMember(destination => destination.HqtfConsiderationAdditionalInformation,
				options => options.MapFrom(source => source.Details.HqtfConsiderationAdditionalInformation))
			.ForMember(destination => destination.HqtfConsiderationSupportingEvidence,
				options => options.MapFrom(source => source.Details.HqtfConsiderationSupportingEvidence))
			.ForMember(destination => destination.HqtfConsiderationTaskStatus,
				options => options.MapFrom(source => source.Details.GetHqtfConsiderationTaskStatus().ToString()))
			.ForMember(destination => destination.EqualitiesImpactAssessmentCompleted,
				options => options.MapFrom(source => source.Details.EqualitiesImpactAssessmentCompleted))
            .ForMember(destination => destination.EqualitiesImpactAssessmentNoReason,
                options => options.MapFrom(source => source.Details.EqualitiesImpactAssessmentNoReason))
			.ForMember(destination => destination.EqualitiesImpactIdentified,
				options => options.MapFrom(source => source.Details.EqualitiesImpactIdentified.ToString()))
			.ForMember(destination => destination.EqualitiesLikelyDetails,
				options => options.MapFrom(source => source.Details.EqualitiesLikelyDetails))
			.ForMember(destination => destination.EqualitiesSomeImpactDetails,
				options => options.MapFrom(source => source.Details.EqualitiesSomeImpactDetails))
			.ForMember(destination => destination.EqualitiesImpactSupportingEvidence,
				options => options.MapFrom(source => source.Details.EqualitiesImpactSupportingEvidence))
			.ForMember(destination => destination.EqualitiesTaskStatus,
				options => options.MapFrom(source => source.Details.GetEqualitiesTaskStatus().ToString()))
			.ForMember(destination => destination.TrustConsultedReligiousBody,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBodyAnswer))
			.ForMember(destination => destination.TrustConsultedReligiousBodyNotConsultedReason,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBodyNotConsultedReason))
			.ForMember(destination => destination.ReligiousBodyConsultationTaskStatus,
				options => options.MapFrom(source => source.Details.GetReligiousBodyConsultationTaskStatus().ToString()))
			.ForMember(destination => destination.ReligiousBodyRaisedObjection,
				options => options.MapFrom(source => source.Details.ReligiousBodyRaisedObjection))
			.ForMember(destination => destination.ReligiousBodyRaisedObjectionReason,
				options => options.MapFrom(source => source.Details.ReligiousBodyRaisedObjectionReason))
			.ForMember(destination => destination.ReligiousBodyConsentGranted,
				options => options.MapFrom(source => source.Details.ReligiousBodyConsentGranted))
			.ForMember(destination => destination.ReligiousBodyConsentNotGrantedReason,
				options => options.MapFrom(source => source.Details.ReligiousBodyConsentNotGrantedReason))
			.ForMember(destination => destination.ReligiousBodySupportingEvidence,
				options => options.MapFrom(source => source.Details.ReligiousBodySupportingEvidence))
			.ForMember(destination => destination.WereThereAnyObjectionsRaised,
				options => options.MapFrom(source => source.Details.WereThereAnyObjectionsRaised))
			.ForMember(destination => destination.StakeholderObjectionsDetails,
				options => options.MapFrom(source => source.Details.StakeholderObjectionsDetails))
			.ForMember(destination => destination.RelevantStakeholdersNotifiedOfAdmissionsChanges,
				options => options.MapFrom(source => source.Details.RelevantStakeholdersNotifiedOfAdmissionsChanges))
			.ForMember(destination => destination.RelevantStakeholdersNotifiedOfAdmissionsChangesReason,
				options => options.MapFrom(source => source.Details.RelevantStakeholdersNotifiedOfAdmissionsChangesReason))
			.ForMember(destination => destination.OtherStakeholderEngagementSupportingEvidence,
				options => options.MapFrom(source => source.Details.OtherStakeholderEngagementSupportingEvidence))
			.ForMember(destination => destination.OtherStakeholderEngagementTaskStatus,
				options => options.MapFrom(source => source.Details.GetOtherStakeholderEngagementTaskStatus().ToString()))
			.ForMember(destination => destination.ProposedChangeDate,
				options => options.MapFrom(source => source.Details.ProposedChangeDate))
			.ForMember(destination => destination.ProposedDecisionDate,
				options => options.MapFrom(source => source.Details.ProposedDecisionDate))
			.ForMember(destination => destination.ConfirmProjectDatesTaskStatus,
				options => options.MapFrom(source => source.Details.GetConfirmProjectDatesTaskStatus().ToString()))
			.ForMember(destination => destination.Recommendation,
				options => options.MapFrom(source => source.Details.Recommendation))
			.ForMember(destination => destination.RecommendationMoreInformation,
				options => options.MapFrom(source => source.Details.RecommendationMoreInformation))
			.ForMember(destination => destination.RecommendationTaskStatus,
				options => options.MapFrom(source => source.Details.GetRecommendationTaskStatus().ToString()))
			.ForMember(destination => destination.PlanningPermission,
				options => options.MapFrom(source => source.Details.PlanningPermission))
			.ForMember(destination => destination.PlanningPermissionAdditionalInformation,
				options => options.MapFrom(source => source.Details.PlanningPermissionAdditionalInformation))
			.ForMember(destination => destination.PlanningPermissionSupportingEvidence,
				options => options.MapFrom(source => source.Details.PlanningPermissionSupportingEvidence))
			.ForMember(destination => destination.PlanningPermissionTaskStatus,
				options => options.MapFrom(source => source.Details.GetPlanningPermissionTaskStatus().ToString()))
			.ForMember(destination => destination.AdmissionsVariationRecommendationTaskStatus,
				options => options.MapFrom(source =>
					source.Details.GetAdmissionsVariationRecommendationTaskStatus().ToString()))
			.ForMember(destination => destination.AdmissionsVariationRecommendation,
				options => options.MapFrom(source => source.Details.AdmissionsVariationRecommendation))
			.ForMember(destination => destination.AdmissionsVariationRequired,
				options => options.MapFrom(source => source.Details.AdmissionsVariationRequired))
			.ForMember(destination => destination.AdmissionsVariationSupportingEvidence,
				options => options.MapFrom(source => source.Details.AdmissionsVariationSupportingEvidence))
			.ForMember(destination => destination.AdmissionsVariationLocalAuthoritySupportive,
				options => options.MapFrom(source => source.Details.AdmissionsVariationLocalAuthoritySupportive))
			.ForMember(destination => destination.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation,
				options => options.MapFrom(source => source.Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportive,
				options => options.MapFrom(source => source.Details.AdmissionsVariationPupilPlacePlanningSupportive))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
				options => options.MapFrom(source => source.Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportive,
				options => options.MapFrom(source => source.Details.AdmissionsVariationAdmissionsTeamSupportive))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation,
				options => options.MapFrom(source => source.Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationTaskStatus,
				options => options.MapFrom(source => source.Details.GetAdmissionVariationTaskStatus().ToString()));


		
		CreateMap<SignificantChangeProjectDto, SignificantChangeConsultationDurationResponse>()
			.ForMember(destination => destination.ConsultationLastedMinimumThreeWeeks,
				options => options.MapFrom(source => (int?)source.ConsultationLastedMinimumThreeWeeks))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConsultationDurationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeFundingResponse>()
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.FundingAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.FundingSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.FundingTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeHqtfConsiderationResponse>()
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.HqtfConsiderationAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.HqtfConsiderationSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.HqtfConsiderationTaskStatus));

        CreateMap<SignificantChangeProjectDto, EqualitiesImpactAssessmentResponse>()
            .ForMember(destination => destination.Status,
                options => options.MapFrom(source => source.EqualitiesTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeReligiousBodyConsultationResponse>()
			.ForMember(destination => destination.TrustConsultedReligiousBody,
				options => options.MapFrom(source => (int?)source.TrustConsultedReligiousBody))
			.ForMember(destination => destination.ReligiousBodyRaisedObjection,
				options => options.MapFrom(source => (int?)source.ReligiousBodyRaisedObjection))
			.ForMember(destination => destination.ReligiousBodyConsentGranted,
				options => options.MapFrom(source => (int?)source.ReligiousBodyConsentGranted))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.ReligiousBodySupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ReligiousBodyConsultationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeOtherStakeholderEngagementResponse>()
			.ForMember(destination => destination.RelevantStakeholdersNotifiedOfAdmissionsChanges,
				options => options.MapFrom(source => (int?)source.RelevantStakeholdersNotifiedOfAdmissionsChanges))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.OtherStakeholderEngagementSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.OtherStakeholderEngagementTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangePlanningPermissionResponse>()
			.ForMember(destination => destination.PlanningPermissionAnswer,
				options => options.MapFrom(source => source.PlanningPermission))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.PlanningPermissionAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.PlanningPermissionSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.PlanningPermissionTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeAdmissionsVariationRecommendationResponse>()
			.ForMember(destination => destination.AdmissionsVariationRecommendationAnswer,
				options => options.MapFrom(source => source.AdmissionsVariationRecommendation))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.AdmissionsVariationRecommendationTaskStatus));
		CreateMap<SignificantChangeProjectDto, SignificantChangeRecommendationResponse>()
			.ForMember(destination => destination.Recommendation,
				options => options.MapFrom(source => source.Recommendation))
			.ForMember(destination => destination.RecommendationMoreInformation,
				options => options.MapFrom(source => source.RecommendationMoreInformation))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.RecommendationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeAdmissionVariationResponse>()
			.ForMember(destination => destination.AdmissionsVariationRequired,
				options => options.MapFrom(source => source.AdmissionsVariationRequired))
			.ForMember(destination => destination.AdmissionsVariationSupportingEvidence,
				options => options.MapFrom(source => source.AdmissionsVariationSupportingEvidence))
			.ForMember(destination => destination.AdmissionsVariationLocalAuthoritySupportive,
				options => options.MapFrom(source => source.AdmissionsVariationLocalAuthoritySupportive))
			.ForMember(destination => destination.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation,
				options => options.MapFrom(source => source.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportive,
				options => options.MapFrom(source => source.AdmissionsVariationPupilPlacePlanningSupportive))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
				options => options.MapFrom(source => source.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportive,
				options => options.MapFrom(source => source.AdmissionsVariationAdmissionsTeamSupportive))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation,
				options => options.MapFrom(source => source.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.AdmissionsVariationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectSearchResponse>()
			.ForMember(destination => destination.ApplicationId,
				options => options.MapFrom(source => source.ApplicationId ?? string.Empty))
			.ForMember(destination => destination.ApplicationReference,
				options => options.MapFrom(source => source.ApplicationReference ?? string.Empty))
			.ForMember(destination => destination.AssignedUser,
				options => options.MapFrom(source => source.AssignedUserId == null
					? null
					: new User(
						source.AssignedUserId.Value,
						source.AssignedUserFullName ?? string.Empty,
						source.AssignedUserEmailAddress ?? string.Empty)));

		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectGetByIdResponse>()
			.IncludeBase<SignificantChangeProjectDto, SignificantChangeProjectSearchResponse>()
			.ForMember(destination => destination.ConsultationDuration,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.EqualitiesImpactAssessment,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.Funding,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.HqtfConsideration,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ReligiousBodyConsultation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.OtherStakeholderEngagement,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ProjectDates,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.PlanningPermission,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.AdmissionsVariationRecommendation,
      	options => options.MapFrom(source => source))
			.ForMember(destination => destination.Recommendation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.AdmissionsVariation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.Decision,
        		options => options.Ignore())
    		.ForMember(destination => destination.DecisionDate,
        		options => options.Ignore());


		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectDatesResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConfirmProjectDatesTaskStatus));

	}
}
