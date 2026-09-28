using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Complete.Client.Contracts;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteConversionProjectServiceModelMapper
{
	internal static CreateSignificantChangeProjectCommand FromDomain(SignificantChangeProject significantChangeProject, ConversionAdvisoryBoardDecision? conversionAdvisoryBoardDecision)
	{
		int? trustUkprn = int.TryParse(significantChangeProject.TrustUkprn, out int parsedTrustUkprn) ? parsedTrustUkprn : null;

		var assignedUser = significantChangeProject.AssignedUserFullName;

		var (firstName, lastName) = assignedUser.GetFirstAndLastName();

		return new CreateSignificantChangeProjectCommand
		{
			PrepareId = significantChangeProject.Id,
			AcademyUrn = significantChangeProject.Urn, 
			TrustUkprn = trustUkprn,
			DecisionConditions = conversionAdvisoryBoardDecision?.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails,
			DecisionRecordedByEmail = significantChangeProject.AssignedUserEmailAddress,
			DecisionRecordedByFirstName = firstName,
			DecisionRecordedByLastName = lastName
		};
	}

	internal static CreateConversionProjectCommand FromDomain(IProject project, string conditions, string groupReferenceNumber, DateTime? advisoryBoardDecisionDate)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		var (firstName, lastName) = fullName.GetFirstAndLastName();

		return new CreateConversionProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = advisoryBoardDecisionDate,
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

	internal static CreateConversionMatProjectCommand FormAMatFromDomain(IProject project, string conditions, string groupReferenceNumber, DateTime? advisoryBoardDecisionDate)
	{
		var assignedUser = project.Details.AssignedUser;
		 
		string? fullName = assignedUser?.FullName;
		var (firstName, lastName) = fullName.GetFirstAndLastName();
		return new CreateConversionMatProjectCommand
		{
			Urn = project.Details.Urn,
			AdvisoryBoardDate = advisoryBoardDecisionDate,
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
