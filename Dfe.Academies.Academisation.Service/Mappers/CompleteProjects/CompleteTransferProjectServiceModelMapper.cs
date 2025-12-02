using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Complete.Client.Contracts;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteTransferProjectServiceModelMapper
{	
	internal static CreateTransferProjectCommand FromDomain(ITransferProject project, string conditions, string urn)
	{

		string incomingTrustUkprn = project.TransferringAcademies.First().IncomingTrustUkprn!;
		
		
		bool outGoingTrustToClose = project.SpecificReasonsForTransfer.Contains("VoluntaryClosure") ||
		                            project.SpecificReasonsForTransfer.Contains("VoluntaryClosureIntervention") ||
		                            project.SpecificReasonsForTransfer.Contains("InterventionClosure");

		bool inadequeteOfsted = project.SpecificReasonsForTransfer.Contains("Forced");
		
		bool financialSafeGuardingOrGovernanceIssues = project.SpecificReasonsForTransfer.Contains("Finance") ||
		                                               project.SpecificReasonsForTransfer.Contains("Safeguarding") ||
		                                               project.SpecificReasonsForTransfer.Contains("TrustClosed");
		
		string? fullName = project.AssignedUserFullName ?? null;
		string[] nameParts = fullName?
			.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			?? [];

		return new CreateTransferProjectCommand
		{
			Urn = int.Parse(urn),
			AdvisoryBoardDate = project.HtbDate,
			AdvisoryBoardConditions = conditions,
			ProvisionalTransferDate = project.TargetDateForTransfer,
			InadequateOfsted = inadequeteOfsted,
			FinancialSafeguardingGovernanceIssues = financialSafeGuardingOrGovernanceIssues,
			OutgoingTrustToClose = outGoingTrustToClose,
			CreatedByEmail = project.AssignedUserEmailAddress,
			CreatedByFirstName = nameParts.Length > 0 ? nameParts[0] : null,
			CreatedByLastName = nameParts.Length > 1 ? nameParts[1] : null,
			IncomingTrustUkprn = int.Parse(incomingTrustUkprn),
			OutgoingTrustUkprn = int.Parse(project.OutgoingTrustUkprn),
			PrepareId = project.Id
		};
	}

	internal static CreateTransferMatProjectCommand FormAMatFromDomain(ITransferProject project, string conditions, string urn)
	{
		string incomingName = project.TransferringAcademies.First().IncomingTrustName!;


		bool outGoingTrustToClose = project.SpecificReasonsForTransfer.Contains("VoluntaryClosure") ||
									project.SpecificReasonsForTransfer.Contains("VoluntaryClosureIntervention") ||
									project.SpecificReasonsForTransfer.Contains("InterventionClosure");

		bool inadequeteOfsted = project.SpecificReasonsForTransfer.Contains("Forced");

		bool financialSafeGuardingOrGovernanceIssues = project.SpecificReasonsForTransfer.Contains("Finance") ||
													   project.SpecificReasonsForTransfer.Contains("Safeguarding") ||
													   project.SpecificReasonsForTransfer.Contains("TrustClosed");

		string? fullName = project.AssignedUserFullName ?? null;
		string[] nameParts = fullName?
			.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			?? []; 

		return new CreateTransferMatProjectCommand
		{
			 Urn = int.Parse(urn),
			 AdvisoryBoardDate = project.HtbDate,
			  AdvisoryBoardConditions = conditions,
			  ProvisionalTransferDate = project.TargetDateForTransfer,
			  InadequateOfsted = inadequeteOfsted,
			  FinancialSafeguardingGovernanceIssues = financialSafeGuardingOrGovernanceIssues,
			  OutgoingTrustToClose = outGoingTrustToClose,
			  CreatedByEmail = project.AssignedUserEmailAddress,
			  CreatedByFirstName = nameParts.Length > 0 ? nameParts[0] : null,
			  CreatedByLastName = nameParts.Length > 1 ? nameParts[1] : null,
			  OutgoingTrustUkprn = int.Parse(project.OutgoingTrustUkprn),
			  PrepareId = project.Id,
			  NewTrustReferenceNumber = project.IncomingTrustReferenceNumber,
			  NewTrustName = incomingName
		};
	}
}
