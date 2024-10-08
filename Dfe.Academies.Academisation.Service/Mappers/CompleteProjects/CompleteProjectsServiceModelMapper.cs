﻿using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteProjectsServiceModelMapper
{
	internal static CompleteProjectsServiceModel FromDomain(IProject project, string conditions, string groupReferenceNumber)
	{
		var assignedUser = project.Details.AssignedUser;

		string? email = assignedUser != null ? assignedUser.EmailAddress : null;
		string? fullName = assignedUser != null ? assignedUser.FullName : null;
		string? firstName = fullName != null ? fullName.Split(' ')[0] : null;
		string? lastName = fullName != null ? fullName.Split(' ')[1] : null;
		
		return new CompleteProjectsServiceModel(
			project.Details.Urn,
			project.Details.HeadTeacherBoardDate.ToString(),
			conditions,
			project.Details.ProposedConversionDate.ToString(),
			true,
			email,
			firstName,
			lastName,
			project.Id,
			groupReferenceNumber,
			project.Details.TrustUkprn
		);
	}
}
