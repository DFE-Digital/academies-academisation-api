using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IApplication
{
	int ApplicationId { get; }
	int Id { get; }
	Guid EntityId { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }

	ApplicationType ApplicationType { get; }
	ApplicationStatus ApplicationStatus { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }

	IReadOnlyCollection<ISchool> Schools { get; }
	IFormTrust? FormTrust { get;  }
	IJoinTrust? JoinTrust { get; }

	DateTime? ApplicationSubmittedDate { get;  }

	/// <summary>
	/// This is in the format $"A2B_{ApplicationId}"
	/// Currently calculated by new UI but we need somewhere to store existing data from dynamics
	/// </summary>
	string? ApplicationReference { get; set; }

	DateTime? DeletedAt  { get; set; }

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
	CommandResult Submit(DateTime submissionDate);

	CommandResult CreateLoan(int schoolId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule);
	CommandResult UpdateLoan(int schoolId, int loanId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule);
	CommandResult DeleteLoan(int schoolId, int loanId);
	CommandResult CreateLease(int schoolId, string leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
	CommandResult UpdateLease(int schoolId, int leaseId, string leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
	CommandResult DeleteLease(int schoolId, int leaseId);
	CommandResult SetJoinTrustDetails(int UKPRN, string trustName, string trustReference, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained);
	CommandResult SetFormTrustDetails(FormTrustDetails formTrustDetails);
	CommandResult AddTrustKeyPerson(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);
	CommandResult UpdateTrustKeyPerson(int keyPersonId, string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);
	CommandResult DeleteTrustKeyPerson(int keyPersonId);
	CommandResult DeleteSchool(int urn);
	CommandResult ValidateSoftDelete(int applicationId);

	CommandResult SetAdditionalDetails(
		int schoolId,
		string trustBenefitDetails,
		string? ofstedInspectionDetails,
		bool safeguarding,
		string? localAuthorityReorganisationDetails,
		string? localAuthorityClosurePlanDetails,
		string? dioceseName,
		string dioceseFolderIdentifier,
		bool partOfFederation,
		string? foundationTrustOrBodyName,
		string foundationConsentFolderIdentifier,
		DateTimeOffset? exemptionEndDate,
		string mainFeederSchools,
		string resolutionConsentFolderIdentifier,
		SchoolEqualitiesProtectedCharacteristics? protectedCharacteristics,
		string? furtherInformation);
}
