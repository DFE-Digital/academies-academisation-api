using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Complete.Client.Contracts;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteConversionProjectServiceModelMapper
{
	internal static CreateConversionProjectCommand FromDomain(IProject project, string conditions, string groupReferenceNumber)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		string[] nameParts = fullName?
			.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			?? [];

		return new CreateConversionProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = project.Details.HeadTeacherBoardDate,
			AdvisoryBoardConditions = conditions,
			ProvisionalConversionDate = project.Details.ProposedConversionDate,
			DirectiveAcademyOrder = project.Details.AcademyTypeAndRoute?.Equals("Sponsored") ?? false,
			CreatedByEmail = assignedUser?.EmailAddress,
			CreatedByFirstName = nameParts.Length > 0 ? nameParts[0] : null,
			CreatedByLastName = nameParts.Length > 1 ? nameParts[1] : null,
			PrepareId = project.Id,
			GroupId = groupReferenceNumber,
			IncomingTrustUkprn = project.Details.TrustUkprn
		}; 
	}

	internal static CreateConversionMatProjectCommand FormAMatFromDomain(IProject project, string conditions, string groupReferenceNumber)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		string[] nameParts = fullName?
			.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			?? [];
		return new CreateConversionMatProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = project.Details.HeadTeacherBoardDate,
			AdvisoryBoardConditions = conditions,
			ProvisionalConversionDate = project.Details.ProposedConversionDate,
			DirectiveAcademyOrder = project.Details.AcademyTypeAndRoute?.Equals("Sponsored") ?? false,
			CreatedByEmail = assignedUser?.EmailAddress,
			CreatedByFirstName = nameParts.Length > 0 ? nameParts[0] : null,
			CreatedByLastName = nameParts.Length > 1 ? nameParts[1] : null,
			PrepareId = project.Id,
			NewTrustName = project.Details.NameOfTrust,
			NewTrustReferenceNumber = project.Details.TrustReferenceNumber,
			GroupId = groupReferenceNumber
		}; 
	}
}
