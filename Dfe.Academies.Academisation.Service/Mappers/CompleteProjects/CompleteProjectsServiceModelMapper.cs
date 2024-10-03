using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteProjectsServiceModelMapper
{
	internal static CompleteProjectsServiceModel FromDomain(IProject project, string conditions)
	{
		
		bool assignedUserPopulated  = (project.Details.AssignedUser != null);
		var createdByEmail =(assignedUserPopulated = true) ? project.Details.AssignedUser.EmailAddress : "";
		var createdByFullName = (assignedUserPopulated = true) ? project.Details.AssignedUser.FullName : "";
		
		
		return new CompleteProjectsServiceModel(
			project.Details.Urn,
			project.Details.HeadTeacherBoardDate.ToString(),
			
			conditions,
			project.Details.ProposedConversionDate.ToString(),
			true,
			createdByEmail,
			createdByFullName,
			createdByFullName,
			project.Id,
			project.ProjectGroupId.ToString(),
			Int32.Parse("1233444")
		);
	}
}
