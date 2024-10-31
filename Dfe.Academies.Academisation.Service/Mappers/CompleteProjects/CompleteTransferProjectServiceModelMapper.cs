using System.Globalization;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;

namespace Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;

internal static class CompleteTransferProjectServiceModelMapper
{	
	internal static CompleteTransferProjectServiceModel FromDomain(ITransferProject project, string conditions, string urn)
	{

		string incomingTrustUkprn = project.TransferringAcademies.First().IncomingTrustUkprn;
		
		
		bool outGoingTrustToClose = project.SpecificReasonsForTransfer.Contains("VoluntaryClosure") ||
		                            project.SpecificReasonsForTransfer.Contains("VoluntaryClosureIntervention") ||
		                            project.SpecificReasonsForTransfer.Contains("InterventionClosure");

		bool inadequeteOfsted = project.SpecificReasonsForTransfer.Contains("Forced");
		
		bool financialSafeGuardingOrGovernanceIssues = project.SpecificReasonsForTransfer.Contains("Finance") ||
		                                               project.SpecificReasonsForTransfer.Contains("Safeguarding") ||
		                                               project.SpecificReasonsForTransfer.Contains("TrustClosed");
		
		string? fullName = project.AssignedUserFullName != null ? project.AssignedUserFullName : null;
		string? firstName = fullName != null ? fullName.Split(' ')[0] : null;
		string? lastName = fullName != null ? fullName.Split(' ')[1] : null;
		
		return new CompleteTransferProjectServiceModel(
			int.Parse(urn),
			project.HtbDate?.ToString(new CultureInfo("en-GB")),
			conditions,
			project.TargetDateForTransfer?.ToString(new CultureInfo("en-GB")),
			inadequeteOfsted,
			financialSafeGuardingOrGovernanceIssues,
			outGoingTrustToClose,
			project.AssignedUserEmailAddress,
			firstName,
			lastName,
			project.Id,
			null,
			int.Parse(incomingTrustUkprn),
			int.Parse(project.OutgoingTrustUkprn)
			
		);
	}

	internal static CompleteFormAMatTransferProjectServiceModel FormAMatFromDomain(ITransferProject project, string conditions, string urn)
	{
		string incomingName = project.TransferringAcademies.First().IncomingTrustName;


		bool outGoingTrustToClose = project.SpecificReasonsForTransfer.Contains("VoluntaryClosure") ||
									project.SpecificReasonsForTransfer.Contains("VoluntaryClosureIntervention") ||
									project.SpecificReasonsForTransfer.Contains("InterventionClosure");

		bool inadequeteOfsted = project.SpecificReasonsForTransfer.Contains("Forced");

		bool financialSafeGuardingOrGovernanceIssues = project.SpecificReasonsForTransfer.Contains("Finance") ||
													   project.SpecificReasonsForTransfer.Contains("Safeguarding") ||
													   project.SpecificReasonsForTransfer.Contains("TrustClosed");

		string? fullName = project.AssignedUserFullName != null ? project.AssignedUserFullName : null;
		string? firstName = fullName != null ? fullName.Split(' ')[0] : null;
		string? lastName = fullName != null ? fullName.Split(' ')[1] : null;

		return new CompleteFormAMatTransferProjectServiceModel(
			int.Parse(urn),
			project.HtbDate?.ToString(new CultureInfo("en-GB")),
			conditions,
			project.TargetDateForTransfer?.ToString(new CultureInfo("en-GB")),
			inadequeteOfsted,
			financialSafeGuardingOrGovernanceIssues,
			int.Parse(project.OutgoingTrustUkprn),
			outGoingTrustToClose,
			project.AssignedUserEmailAddress,
			firstName,
			lastName,
			project.Id,
			// Transfer projects aren't currently added to groups
			null,
			// Need the trust reference number, proposed trust name is held in the incoming trust name field
			project.IncomingTrustReferenceNumber,
			incomingName			
		);
	}
}
