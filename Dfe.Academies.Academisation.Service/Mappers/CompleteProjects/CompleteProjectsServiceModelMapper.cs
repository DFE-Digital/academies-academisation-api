using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteProjectsServiceModelMapper
{
	internal static CompleteProjectsServiceModel FromDomain(IProject project, string conditions)
	{
		
		//bool assignedUserPopulated  = (project.Details.AssignedUser != null);
		//var createdByEmail =(assignedUserPopulated = true) ? project.Details.AssignedUser.EmailAddress : "dave";
		//var createdByFullName = (assignedUserPopulated = true) ? project.Details.AssignedUser.FullName : "dave";
		
		
		return new CompleteProjectsServiceModel(
			127488,
			"01/12/2023 00:00:00",
			
			conditions,
			"01/12/2029 00:00:00",
			true,
			"createdByEmai@education.gov.uk",
			"createdByFullName",
			"createdByFullName",
			project.Id,
			project.ProjectGroupId.ToString(),
			10061064
		);
	}
}
