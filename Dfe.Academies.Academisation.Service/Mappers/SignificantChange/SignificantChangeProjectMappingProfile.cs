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
			.ForMember(destination => destination.ConsultStakeholdersTaskStatus,
				options => options.MapFrom(source => source.Details.GetConsultStakeholdersTaskStatus().ToString()));

		CreateMap<SignificantChangeProjectDto, SignificantChangeConsultStakeholdersResponse>()
			.ForMember(destination => destination.Status,
				options => options.MapFrom(source => source.ConsultStakeholdersTaskStatus));

		CreateMap<SignificantChangeProjectDto, SignificantChangeProjectSearchResponse>()
			.ForMember(destination => destination.AssignedUser,
				options => options.MapFrom(source => source.AssignedUserId == null
					? null
					: new User(
						source.AssignedUserId.Value,
						source.AssignedUserFullName ?? string.Empty,
						source.AssignedUserEmailAddress ?? string.Empty)))
			.ForMember(destination => destination.ConsultStakeholders,
				options => options.MapFrom(source => source));
	}
}