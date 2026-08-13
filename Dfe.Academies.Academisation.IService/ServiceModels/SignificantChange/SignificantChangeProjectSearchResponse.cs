using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
public class SignificantChangeProjectSearchResponse : SignificantChangeProjectResponse
{
    public User? AssignedUser { get; set; }
	public SignificantChangeStakeholderConsultationResponse StakeholderConsultation { get; set; } = new();
}
