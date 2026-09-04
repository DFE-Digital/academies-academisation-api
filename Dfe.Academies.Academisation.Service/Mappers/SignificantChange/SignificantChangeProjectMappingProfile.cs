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
            .ForMember(destination=> destination.EqualitiesImpactAssessmentCompleted,
                options=>options.MapFrom(source=> source.Details.EqualitiesImpactAssessmentCompleted))
            .ForMember(destination => destination.EqualitiesImpactIdentified,
                options => options.MapFrom(source => source.Details.EqualitiesImpactIdentified.ToString()))
            .ForMember(destination => destination.EqualitiesImpactIdentifiedMitigation,
                options => options.MapFrom(source => source.Details.EqualitiesImpactIdentifiedMitigation))
            .ForMember(destination => destination.EqualitiesTaskStatus,
                options => options.MapFrom(source => source.Details.GetEqualitiesTaskStatus().ToString()));


		CreateMap<SignificantChangeProjectDto, SignificantChangeStakeholderConsultationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.StakeholderConsultationTaskStatus));

        CreateMap<SignificantChangeProjectDto, EqualitiesImpactAssessmentResponse>()
            .ForMember(destination => destination.Status,
                options => options.MapFrom(source => source.EqualitiesTaskStatus));


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
                options => options.MapFrom(source => source));
    }
}
