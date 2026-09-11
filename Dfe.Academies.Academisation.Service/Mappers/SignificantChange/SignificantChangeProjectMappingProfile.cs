using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using DocumentFormat.OpenXml.Wordprocessing;

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
			.ForMember(destination => destination.StakeholderConsultationTaskStatus,
				options => options.MapFrom(source => source.Details.GetStakeholderConsultationTaskStatus().ToString()))

			.ForMember(destination => destination.ConsultationIncludeAdmissionVariation,
				options => options.MapFrom(source => source.Details.ConsultationIncludeAdmissionVariation))
			.ForMember(destination => destination.ConsultationNoAdmissionVariationReason,
				options => options.MapFrom(source => source.Details.ConsultationNoAdmissionVariationReason))
			.ForMember(destination => destination.AdmissionVariationConsultationTaskStatus,
				options => options.MapFrom(source => source.Details.GetAdmissionVariationConsultationTaskStatus().ToString()));
            .ForMember(destination=> destination.EqualitiesImpactAssessmentCompleted,
                options=>options.MapFrom(source=> source.Details.EqualitiesImpactAssessmentCompleted))
            .ForMember(destination => destination.EqualitiesImpactIdentified,
                options => options.MapFrom(source => source.Details.EqualitiesImpactIdentified.ToString()))
            .ForMember(destination => destination.EqualitiesImpactIdentifiedMitigation,
                options => options.MapFrom(source => source.Details.EqualitiesImpactIdentifiedMitigation))
            .ForMember(destination => destination.EqualitiesTaskStatus,
                options => options.MapFrom(source => source.Details.GetEqualitiesTaskStatus().ToString()))
			.ForMember(destination => destination.TrustConsultedReligiousBody,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBody))
			.ForMember(destination => destination.TrustConsultedReligiousBodyNotConsultedReason,
				options => options.MapFrom(source => source.Details.TrustConsultedReligiousBodyNotConsultedReason))
			.ForMember(destination => destination.ReligiousBodyConsultationTaskStatus,
				options => options.MapFrom(source => source.Details.GetReligiousBodyConsultationTaskStatus().ToString()))
			.ForMember(destination => destination.ProposedChangeDate,
				options => options.MapFrom(source => source.Details.ProposedChangeDate))
			.ForMember(destination => destination.ProposedDecisionDate,
				options => options.MapFrom(source => source.Details.ProposedDecisionDate))
			.ForMember(destination => destination.ConfirmProjectDatesTaskStatus,
				options => options.MapFrom(source => source.Details.GetConfirmProjectDatesTaskStatus().ToString()));


		CreateMap<SignificantChangeProjectDto, SignificantChangeStakeholderConsultationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.StakeholderConsultationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeAdmissionVariationConsultationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.AdmissionVariationConsultationTaskStatus));

        CreateMap<SignificantChangeProjectDto, EqualitiesImpactAssessmentResponse>()
            .ForMember(destination => destination.Status,
                options => options.MapFrom(source => source.EqualitiesTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeReligiousBodyConsultationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ReligiousBodyConsultationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectSearchResponse>()
            .ForMember(destination => destination.AssignedUser,
                options => options.MapFrom(source => source.AssignedUserId == null
                    ? null
                    : new User(
                        source.AssignedUserId.Value,
                        source.AssignedUserFullName ?? string.Empty,
                        source.AssignedUserEmailAddress ?? string.Empty)))
            .ForMember(destination => destination.StakeholderConsultation,
                options => options.MapFrom(source => source))
            .ForMember(destination => destination.EqualitiesImpactAssessment,
                options => options.MapFrom(source => source))
			.ForMember(destination => destination.AssignedUser,
				options => options.MapFrom(source => source.AssignedUserId == null
					? null
					: new User(
						source.AssignedUserId.Value,
						source.AssignedUserFullName ?? string.Empty,
						source.AssignedUserEmailAddress ?? string.Empty)))
			.ForMember(destination => destination.StakeholderConsultation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.AdmissionVariationConsultation,
				options => options.MapFrom(source => source));
		
			.ForMember(destination => destination.ReligiousBodyConsultation,
				options => options.MapFrom(source => source))
			.ForMember(destination => destination.ProjectDates,
				options => options.MapFrom(source => source));

		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectDatesResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConfirmProjectDatesTaskStatus));

	}
}
