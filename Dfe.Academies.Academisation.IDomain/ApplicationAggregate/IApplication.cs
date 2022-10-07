﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IApplication
{
	int ApplicationId { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }

	ApplicationType ApplicationType { get; }
	ApplicationStatus ApplicationStatus { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }

	IReadOnlyCollection<ISchool> Schools { get; }
	IFormTrust? FormTrust { get;  }
	IJoinTrust? JoinTrust { get; }

	void SetIdsOnCreate(int applicationId, int conversionId);

	CommandResult Update(
		ApplicationType applicationType,
		ApplicationStatus applicationStatus,
		IEnumerable<KeyValuePair<int, ContributorDetails>> contributors,
		IEnumerable<UpdateSchoolParameter> schools
		);

	/// <summary>
	/// Be careful of using this method outside the Domain Layer - this only submits the Application but doesn't create the Project.
	/// Use the IApplicationSubmissionService to submit the Application and (conditionally) create a Project. 
	/// </summary>
	CommandResult Submit();
	CommandResult SetJoinTrustDetails(int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained);

	CommandResult CreateLoan(int schoolId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule);
	CommandResult UpdateLoan(int schoolId, int loanId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule);
	CommandResult DeleteLoan(int schoolId, int loanId);
	CommandResult CreateLease(int schoolId, int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
	CommandResult UpdateLease(int schoolId, int leaseId, int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
	CommandResult DeleteLease(int schoolId, int leaseId);
}
