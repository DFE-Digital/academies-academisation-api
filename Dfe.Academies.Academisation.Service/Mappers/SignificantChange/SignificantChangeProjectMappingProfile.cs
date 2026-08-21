using AutoMapper;
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
			.ForMember(destination => destination.StakeholderConsultationTaskStatus,
				options => options.MapFrom(source => source.Details.GetStakeholderConsultationTaskStatus().ToString()))
			.ForMember(destination => destination.ConsultationLastedMinimumThreeWeeks,
				options => options.MapFrom(source => source.Details.ConsultationLastedMinimumThreeWeeks))
			.ForMember(destination => destination.ConsultationDurationNotMetReason,
				options => options.MapFrom(source => source.Details.ConsultationDurationNotMetReason))
			.ForMember(destination => destination.ConsultationDurationTaskStatus,
				options => options.MapFrom(source => source.Details.GetConsultationDurationTaskStatus().ToString()));

		CreateMap<SignificantChangeProjectDto, SignificantChangeConsultationDurationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConsultationDurationTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeStakeholderConsultationResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.StakeholderConsultationTaskStatus));

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
			.ForMember(destination => destination.ConsultationDuration,
				options => options.MapFrom(source => source));
	}
}