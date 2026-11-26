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
			.ForMember(destination => destination.FhaRisksOrIssuesRaised,
				options => options.MapFrom(source => source.Details.FhaRisksOrIssuesRaised))
			.ForMember(destination => destination.FhaRisksOrIssuesRaisedAdditionalInformation,
				options => options.MapFrom(source => source.Details.FhaRisksOrIssuesRaisedAdditionalInformation))
			.ForMember(destination => destination.FhaRisksOrIssuesRaisedSupportingEvidence,
				options => options.MapFrom(source => source.Details.FhaRisksOrIssuesRaisedSupportingEvidence))
			.ForMember(destination => destination.FhaRisksOrIssuesRaisedTaskStatus,
				options => options.MapFrom(source => source.Details.GetFhaRisksOrIssuesRaisedTaskStatus().ToString()))
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
			.ForMember(destination => destination.LocalAuthorityRaisedObjections,
				options => options.MapFrom(source => source.Details.LocalAuthorityRaisedObjections))
			.ForMember(destination => destination.LocalAuthorityObjectionsFurtherInformation,
				options => options.MapFrom(source => source.Details.LocalAuthorityObjectionsFurtherInformation))
			.ForMember(destination => destination.SupportingEvidenceLink,
				options => options.MapFrom(source => source.Details.LocalAuthoritySupportingEvidenceLink))
			.ForMember(destination => destination.LocalAuthorityObjectionsTaskStatus,
				options => options.MapFrom(source => source.Details.GetLocalAuthorityObjectionsTaskStatus().ToString()))
			.ForMember(destination => destination.EqualitiesImpactAssessmentCompleted,
				options => options.MapFrom(source => source.Details.EqualitiesImpactAssessmentCompleted))
			.ForMember(destination => destination.EqualitiesImpactIdentified,
				options => options.MapFrom(source => source.Details.EqualitiesImpactIdentified.ToString()))
			.ForMember(destination => destination.EqualitiesTaskStatus,
				options => options.MapFrom(source => source.Details.GetEqualitiesTaskStatus().ToString()))
			.ForMember(destination => destination.TrustConsultedReligiousBody,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBodyAnswer))
			.ForMember(destination => destination.TrustConsultedReligiousBodyNotConsultedReason,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBodyNotConsultedReason))
			.ForMember(destination => destination.ReligiousBodyConsultationTaskStatus,
				options => options.MapFrom(source =>
					source.Details.GetReligiousBodyConsultationTaskStatus().ToString()))
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
				options => options.MapFrom(source =>
					source.Details.RelevantStakeholdersNotifiedOfAdmissionsChangesReason))
			.ForMember(destination => destination.OtherStakeholderEngagementSupportingEvidence,
				options => options.MapFrom(source => source.Details.OtherStakeholderEngagementSupportingEvidence))
			.ForMember(destination => destination.OtherStakeholderEngagementTaskStatus,
				options =>
					options.MapFrom(source => source.Details.GetOtherStakeholderEngagementTaskStatus().ToString()))
			.ForMember(destination => destination.ProposedChangeDate,
				options => options.MapFrom(source => source.Details.ProposedChangeDate))
			.ForMember(destination => destination.ProposedDecisionDate,
				options => options.MapFrom(source => source.Details.ProposedDecisionDate))
			.ForMember(destination => destination.ConfirmProjectDatesTaskStatus,
				options => options.MapFrom(source => source.Details.GetConfirmProjectDatesTaskStatus().ToString()))
			.ForMember(destination => destination.LandTransactionApplication,
				options => options.MapFrom(source => source.Details.LandTransactionApplication.ToString()))
			.ForMember(destination => destination.LandTransactionApplicationAdditionalInfo,
				options => options.MapFrom(source => source.Details.LandTransactionApplicationAdditionalInfo))
			.ForMember(destination => destination.LandTransactionConsent,
				options => options.MapFrom(source => source.Details.LandTransactionConsent.ToString()))
			.ForMember(destination => destination.LandTransactionConsentAdditionalInfo,
				options => options.MapFrom(source => source.Details.LandTransactionConsentAdditionalInfo))
			.ForMember(destination => destination.LandTransactionSupportingEvidence,
				options => options.MapFrom(source => source.Details.LandTransactionSupportingEvidence))
			.ForMember(destination => destination.LandTransactionTaskStatus,
				options => options.MapFrom(source => source.Details.GetLandTransactionTaskStatus().ToString()))
			.ForMember(destination => destination.Recommendation,
				options => options.MapFrom(source => source.Details.Recommendation))
			.ForMember(destination => destination.RecommendationMoreInformation,
				options => options.MapFrom(source => source.Details.RecommendationMoreInformation))
			.ForMember(destination => destination.RecommendationTaskStatus,
				options => options.MapFrom(source => source.Details.GetRecommendationTaskStatus().ToString()))
			.ForMember(destination => destination.RegionsGroupClearance,
				options => options.MapFrom(source => source.Details.RegionsGroupClearance))
			.ForMember(destination => destination.TrustRegulationClearance,
				options => options.MapFrom(source => source.Details.TrustRegulationClearance))
			.ForMember(destination => destination.ClearanceTaskStatus,
				options => options.MapFrom(source => source.Details.GetClearanceTaskStatus().ToString()))
			.ForMember(destination => destination.PlanningPermission,
				options => options.MapFrom(source => source.Details.PlanningPermission))
			.ForMember(destination => destination.PlanningPermissionAdditionalInformation,
				options => options.MapFrom(source => source.Details.PlanningPermissionAdditionalInformation))
			.ForMember(destination => destination.PlanningPermissionSupportingEvidence,
				options => options.MapFrom(source => source.Details.PlanningPermissionSupportingEvidence))
			.ForMember(destination => destination.PlanningPermissionTaskStatus,
				options => options.MapFrom(source => source.Details.GetPlanningPermissionTaskStatus().ToString()))
			.ForMember(destination => destination.OfstedInspectionDate,
				options => options.MapFrom(source => source.Details.OfstedInspectionDate))
			.ForMember(destination => destination.MeetsOfstedStandards,
				options => options.MapFrom(source => source.Details.MeetsOfstedStandards))
			.ForMember(destination => destination.OfstedAdditionalInformation,
				options => options.MapFrom(source => source.Details.OfstedAdditionalInformation))
			.ForMember(destination => destination.OfstedSupportingEvidence,
				options => options.MapFrom(source => source.Details.OfstedSupportingEvidence))
			.ForMember(destination => destination.OfstedTaskStatus,
				options => options.MapFrom(source => source.Details.GetOfstedTaskStatus().ToString()))
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
				options => options.MapFrom(source =>
					source.Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportive,
				options => options.MapFrom(source => source.Details.AdmissionsVariationPupilPlacePlanningSupportive))
			.ForMember(destination => destination.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
				options => options.MapFrom(source =>
					source.Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportive,
				options => options.MapFrom(source => source.Details.AdmissionsVariationAdmissionsTeamSupportive))
			.ForMember(destination => destination.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation,
				options => options.MapFrom(source =>
					source.Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation))
			.ForMember(destination => destination.AdmissionsVariationTaskStatus,
				options => options.MapFrom(source => source.Details.GetAdmissionVariationTaskStatus().ToString()))
			.ForMember(destination => destination.SpecialSchoolSatelliteConsiderationsMeetRequirements,
				options =>
					options.MapFrom(source => source.Details.SpecialSchoolSatelliteConsiderationsMeetRequirements))
			.ForMember(destination => destination.SpecialSchoolSatelliteConsiderationsAdditionalInformation,
				options => options.MapFrom(source =>
					source.Details.SpecialSchoolSatelliteConsiderationsAdditionalInformation))
			.ForMember(destination => destination.SpecialSchoolSatelliteConsiderationsSupportingEvidence,
				options => options.MapFrom(source =>
					source.Details.SpecialSchoolSatelliteConsiderationsSupportingEvidence))
			.ForMember(destination => destination.SpecialSchoolSatelliteConsiderationsTaskStatus,
				options => options.MapFrom(source =>
					source.Details.GetSpecialSchoolSatelliteConsiderationsTaskStatus().ToString()))
			.ForMember(destination => destination.SatelliteSiteArrangementAnswer,
				options => options.MapFrom(source => source.Details.SatelliteSiteArrangementAnswer))
			.ForMember(destination => destination.SatelliteSiteArrangementAdditionalInformation,
				options => options.MapFrom(source => source.Details.SatelliteSiteArrangementAdditionalInformation))
			.ForMember(destination => destination.SatelliteSiteArrangementSupportingEvidence,
				options => options.MapFrom(source => source.Details.SatelliteSiteArrangementSupportingEvidence))
			.ForMember(destination => destination.SatelliteSiteArrangementTaskStatus,
				options => options.MapFrom(source =>
					source.Details.GetSatelliteSiteArrangementTaskStatus().ToString()))
			.ForMember(destination => destination.MainstreamSchoolSatelliteConsiderationsMeetRequirements,
				options => options.MapFrom(source => source.Details.MainstreamSchoolSatelliteConsiderationsMeetRequirements))
			.ForMember(destination => destination.MainstreamSchoolSatelliteConsiderationsAdditionalInformation,
				options => options.MapFrom(source => source.Details.MainstreamSchoolSatelliteConsiderationsAdditionalInformation))
			.ForMember(destination => destination.MainstreamSchoolSatelliteConsiderationsSupportingEvidence,
				options => options.MapFrom(source => source.Details.MainstreamSchoolSatelliteConsiderationsSupportingEvidence))
			.ForMember(destination => destination.MainstreamSchoolSatelliteConsiderationsTaskStatus,
				options => options.MapFrom(source => source.Details.GetMainstreamSchoolSatelliteConsiderationsTaskStatus().ToString()))
			.ForMember(destination => destination.TrustExperienceAndGovernanceAnswer,
				options => options.MapFrom(source => source.Details.TrustExperienceAndGovernanceAnswer))
			.ForMember(destination => destination.TrustExperienceAndGovernanceAdditionalInformation,
				options => options.MapFrom(source => source.Details.TrustExperienceAndGovernanceAdditionalInformation))
			.ForMember(destination => destination.TrustExperienceAndGovernanceSupportingEvidence,
				options => options.MapFrom(source => source.Details.TrustExperienceAndGovernanceSupportingEvidence))
			.ForMember(destination => destination.TrustExperienceAndGovernanceTaskStatus,
				options => options.MapFrom(source => source.Details.GetTrustExperienceAndGovernanceTaskStatus().ToString()))
			.ForMember(destination => destination.LocalSenPlansConflict,
				options => options.MapFrom(source => source.Details.LocalSenPlansConflict))
			.ForMember(destination => destination.LocalSenPlansAdditionalInformation,
				options => options.MapFrom(source => source.Details.LocalSenPlansAdditionalInformation))
			.ForMember(destination => destination.LocalSenPlansSupportingEvidence,
				options => options.MapFrom(source => source.Details.LocalSenPlansSupportingEvidence))
			.ForMember(destination => destination.LocalSenPlansTaskStatus,
				options => options.MapFrom(source => source.Details.GetLocalSenPlansTaskStatus().ToString()))
			.ForMember(destination => destination.ImpactOnSenProvisionImprovesProvision,
				options => options.MapFrom(source => source.Details.ImpactOnSenProvisionImprovesProvision))
			.ForMember(destination => destination.ImpactOnSenProvisionAdditionalInformation,
				options => options.MapFrom(source => source.Details.ImpactOnSenProvisionAdditionalInformation))
			.ForMember(destination => destination.ImpactOnSenProvisionSupportingEvidence,
				options => options.MapFrom(source => source.Details.ImpactOnSenProvisionSupportingEvidence))
			.ForMember(destination => destination.ImpactOnSenProvisionTaskStatus,
				options => options.MapFrom(source => source.Details.GetImpactOnSenProvisionTaskStatus().ToString()));


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

		CreateMap<SignificantChangeProjectDto, SignificantChangeLocalAuthorityObjectionsResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.LocalAuthorityObjectionsTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeHqtfConsiderationResponse>()
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.HqtfConsiderationAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.HqtfConsiderationSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.HqtfConsiderationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeFhaRisksOrIssuesRaisedResponse>()
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.FhaRisksOrIssuesRaisedAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.FhaRisksOrIssuesRaisedSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.FhaRisksOrIssuesRaisedTaskStatus));

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
		
		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectDatesResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConfirmProjectDatesTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeLandTransactionResponse>()
			.ForMember(destination => destination.Status, 
				options => options.MapFrom(source => source.LandTransactionTaskStatus));

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

		CreateMap<SignificantChangeProjectDto, SignificantChangeOfstedInspectionResponse>()
			.ForMember(destination => destination.InspectionDate,
				options => options.MapFrom(source => source.OfstedInspectionDate))
			.ForMember(destination => destination.MeetsOfstedStandards,
				options => options.MapFrom(source => source.MeetsOfstedStandards))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.OfstedAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.OfstedSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.OfstedTaskStatus));

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

		CreateMap<SignificantChangeProjectDto, SignificantChangeClearanceResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ClearanceTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeSpecialSchoolSatelliteConsiderationsResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.SpecialSchoolSatelliteConsiderationsMeetRequirements))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.SpecialSchoolSatelliteConsiderationsAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.SpecialSchoolSatelliteConsiderationsSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.SpecialSchoolSatelliteConsiderationsTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeSatelliteSiteArrangementResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.SatelliteSiteArrangementAnswer))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.SatelliteSiteArrangementAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.SatelliteSiteArrangementSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.SatelliteSiteArrangementTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeMainstreamSchoolSatelliteConsiderationsResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.MainstreamSchoolSatelliteConsiderationsMeetRequirements))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.MainstreamSchoolSatelliteConsiderationsAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.MainstreamSchoolSatelliteConsiderationsSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.MainstreamSchoolSatelliteConsiderationsTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeTrustExperienceAndGovernanceResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.TrustExperienceAndGovernanceAnswer))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.TrustExperienceAndGovernanceAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.TrustExperienceAndGovernanceSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.TrustExperienceAndGovernanceTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeLocalSenPlansResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.LocalSenPlansConflict))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.LocalSenPlansAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.LocalSenPlansSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.LocalSenPlansTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeImpactOnSenProvisionResponse>()
			.ForMember(destination => destination.Answer,
				options => options.MapFrom(source => source.ImpactOnSenProvisionImprovesProvision))
			.ForMember(destination => destination.AdditionalInformation,
				options => options.MapFrom(source => source.ImpactOnSenProvisionAdditionalInformation))
			.ForMember(destination => destination.SupportingEvidence,
				options => options.MapFrom(source => source.ImpactOnSenProvisionSupportingEvidence))
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ImpactOnSenProvisionTaskStatus));

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
			.ForMember(destination => destination.LocalAuthorityObjections,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.HqtfConsideration,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.FhaRisksOrIssuesRaised,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ReligiousBodyConsultation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.OtherStakeholderEngagement,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ProjectDates,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.LandTransaction,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.PlanningPermission,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.OfstedInspection,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.AdmissionsVariationRecommendation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.Clearance,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.Recommendation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.AdmissionsVariation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.Decision,
				options => options.Ignore())
			.ForMember(destination => destination.DecisionDate,
				options => options.Ignore())
			.ForMember(destination => destination.SpecialSchoolSatelliteConsiderations,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.SatelliteSiteArrangement,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.MainstreamSchoolSatelliteConsiderations,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.TrustExperienceAndGovernance,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.LocalSenPlans,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ImpactOnSenProvision,
				options => options.MapFrom(source => source));
	}
}
