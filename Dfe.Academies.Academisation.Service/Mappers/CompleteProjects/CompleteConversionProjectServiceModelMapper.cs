using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Complete.Client.Contracts;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteConversionProjectServiceModelMapper
{
	internal static CreateConversionProjectCommand FromDomain(IProject project, string conditions, string groupReferenceNumber)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		var (firstName, lastName) = fullName.GetFirstAndLastName();

		return new CreateConversionProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = project.Details.HeadTeacherBoardDate,
			AdvisoryBoardConditions = conditions,
			ProvisionalConversionDate = project.Details.ProposedConversionDate,
			DirectiveAcademyOrder = project.Details.AcademyTypeAndRoute?.Equals("Sponsored") ?? false,
			CreatedByEmail = assignedUser?.EmailAddress,
			CreatedByFirstName = firstName,
			CreatedByLastName = lastName,
			PrepareId = project.Id,
			GroupId = groupReferenceNumber,
			IncomingTrustUkprn = project.Details.TrustUkprn
		}; 
	}

	internal static CreateConversionMatProjectCommand FormAMatFromDomain(IProject project, string conditions, string groupReferenceNumber)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		var (firstName, lastName) = fullName.GetFirstAndLastName();
		return new CreateConversionMatProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = project.Details.HeadTeacherBoardDate,
			AdvisoryBoardConditions = conditions,
			ProvisionalConversionDate = project.Details.ProposedConversionDate,
			DirectiveAcademyOrder = project.Details.AcademyTypeAndRoute?.Equals("Sponsored") ?? false,
			CreatedByEmail = assignedUser?.EmailAddress,
			CreatedByFirstName = firstName,
			CreatedByLastName = lastName,
			PrepareId = project.Id,
			NewTrustName = project.Details.NameOfTrust,
			NewTrustReferenceNumber = project.Details.TrustReferenceNumber,
			GroupId = groupReferenceNumber
		}; 
	}
}
