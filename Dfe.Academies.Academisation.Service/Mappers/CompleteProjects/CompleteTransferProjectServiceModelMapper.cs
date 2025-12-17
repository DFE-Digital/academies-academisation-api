using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Extensions;
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
		var (firstName, lastName) = fullName.GetFirstAndLastName();

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
			CreatedByFirstName = firstName,
			CreatedByLastName = lastName,
			IncomingTrustUkprn = string.IsNullOrWhiteSpace(incomingTrustUkprn)?null: int.Parse(incomingTrustUkprn),
			OutgoingTrustUkprn = string.IsNullOrWhiteSpace(project.OutgoingTrustUkprn) ? null : int.Parse(project.OutgoingTrustUkprn),
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

		var (firstName, lastName) = project.AssignedUserFullName.GetFirstAndLastName(); 

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
			  CreatedByFirstName = firstName,
			  CreatedByLastName = lastName,
			  OutgoingTrustUkprn = string.IsNullOrWhiteSpace(project.OutgoingTrustUkprn) ? null : int.Parse(project.OutgoingTrustUkprn),
			  PrepareId = project.Id,
			  NewTrustReferenceNumber = project.IncomingTrustReferenceNumber,
			  NewTrustName = incomingName
		};
	}
}
