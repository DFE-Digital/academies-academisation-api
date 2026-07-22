using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
public class SignificantChangeProjectSearchResponse : SignificantChangeProjectResponse
{
    public required string SchoolName { get; set; }
    public User? AssignedUser { get; set; }
}
