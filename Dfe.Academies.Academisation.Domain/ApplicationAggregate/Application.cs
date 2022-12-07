﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.Validations;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class Application : IApplication, IAggregateRoot
{
	private readonly List<Contributor> _contributors = new();
	private readonly List<School> _schools = new();
	private readonly SubmitApplicationValidator submitValidator = new();
	private readonly UpdateApplicationValidator updateValidator = new();
	private readonly SetJoinTrustDetailsValidator setJoinTrustDetailsValidator = new();
	private readonly SetFormTrustDetailsValidator setformJoinTrustDetailsValidator = new();

	private Application(ApplicationType applicationType, ContributorDetails initialContributor)
	{
		ApplicationStatus = ApplicationStatus.InProgress;
		ApplicationType = applicationType;
		_contributors.Add(new(initialContributor));
	}

	public Application(
		int applicationId,
		DateTime createdOn,
		DateTime lastModifiedOn,
		ApplicationType applicationType,
		ApplicationStatus applicationStatus,
		Dictionary<int, ContributorDetails> contributors,
		IEnumerable<School> schools,
		IJoinTrust? joinTrust,
		IFormTrust? formTrust,
		DateTime? applicationSubmittedOn = null,
		string? applicationReference = null)
	{
		ApplicationId = applicationId;
		CreatedOn = createdOn;
		LastModifiedOn = lastModifiedOn;
		ApplicationType = applicationType;
		ApplicationStatus = applicationStatus;
		_contributors = contributors.Select(c => new Contributor(c.Key, c.Value)).ToList();
		_schools = schools.ToList();
		JoinTrust = joinTrust;
		FormTrust = formTrust;
		ApplicationSubmittedDate = applicationSubmittedOn;
		ApplicationReference = applicationReference;
	}

	public int ApplicationId { get; private set; }
	public DateTime CreatedOn { get; }
	public DateTime LastModifiedOn { get; }
	public ApplicationType ApplicationType { get; }
	public ApplicationStatus ApplicationStatus { get; private set; }
	public DateTime? ApplicationSubmittedDate { get; private set; }
	public IFormTrust? FormTrust { get; private set; }
	public IJoinTrust? JoinTrust { get; private set; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	public IReadOnlyCollection<ISchool> Schools => _schools.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		ApplicationId = applicationId;
		_contributors.Single().Id = contributorId;
	}

	/// <summary>
	/// This is in the format $"A2B_{ApplicationId}"
	/// Currently calculated by new UI but we need somewhere to store existing data from dynamics
	/// </summary>
	public string? ApplicationReference { get; set; }

	public CommandResult Update(
		ApplicationType applicationType,
		ApplicationStatus applicationStatus,
		IEnumerable<KeyValuePair<int, ContributorDetails>> contributors,
		IEnumerable<UpdateSchoolParameter> schools)
	{
		var validationResult = updateValidator.Validate((applicationType, applicationStatus, contributors, schools, this));

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		_contributors.RemoveAll(c => true);

		foreach (var contributor in contributors)
		{
			_contributors.Add(new Contributor(
				contributor.Key,
				contributor.Value
				));
		}
		
		_schools.RemoveAll(s => true);

		foreach (var school in schools)
		{
			_schools.Add(new School(
				school.Id, 
				school.TrustBenefitDetails,
				school.OfstedInspectionDetails,
				school.SafeguardingDetails,
				school.LocalAuthorityReorganisationDetails,
				school.LocalAuthorityClosurePlanDetails,
				school.DioceseName,
				school.DioceseFolderIdentifier,
				school.PartOfFederation,
				school.FoundationTrustOrBodyName,
				school.FoundationConsentFolderIdentifier,
				school.ExemptionEndDate,
				school.MainFeederSchools,
				school.ResolutionConsentFolderIdentifier,
				school.ProtectedCharacteristics,
				school.FurtherInformation,
				school.SchoolDetails,
				school.Loans.Select(l => new Loan(l.Key, l.Value.Amount!.Value, l.Value.Purpose!, l.Value.Provider!, l.Value.InterestRate!.Value, l.Value.Schedule!)),
				school.Leases.Select(l => new Lease(l.Key, l.Value.leaseTerm, l.Value.repaymentAmount, l.Value.interestRate, l.Value.paymentsToDate, l.Value.purpose, l.Value.valueOfAssets, l.Value.responsibleForAssets)),
				school.HasLoans, school.HasLeases));
		}
		
		return new CommandSuccessResult();
	}

	public CommandResult Submit(DateTime submissionDate)
	{
		var validationResult = submitValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		ApplicationStatus = ApplicationStatus.Submitted;
		ApplicationSubmittedDate = submissionDate;

		return new CommandSuccessResult();
	}

	internal static CreateResult<IApplication> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		var validationResult = new CreateApplicationValidator().Validate(initialContributor);

		if (!validationResult.IsValid)
		{
			return new CreateValidationErrorResult<IApplication>(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		return new CreateSuccessResult<IApplication>(new Application(applicationType, initialContributor));
	}

	public CommandResult SetJoinTrustDetails(int UKPRN, string trustName, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained)
	{
		// check the application type allows join trust details to be set
		var validationResult = setJoinTrustDetailsValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		// if the trust is already set update the fields
		if (JoinTrust != null)
		{
			JoinTrust.Update(UKPRN, trustName, changesToTrust, changesToTrustExplained, changesToLaGovernance, changesToLaGovernanceExplained);

		}
		else { 
			JoinTrust = Trusts.JoinTrust.Create(UKPRN, trustName, changesToTrust, changesToTrustExplained, changesToLaGovernance, changesToLaGovernanceExplained); 
		}

		return new CommandSuccessResult();
	}

	public CommandResult CreateLoan(int schoolId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		if(school == null) return new NotFoundCommandResult();
		
		school.AddLoan(amount, purpose, provider, interestRate, schedule);
		return new CommandSuccessResult();
	}

	public CommandResult UpdateLoan(int schoolId, int loanId, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		var loan = school?.Loans.FirstOrDefault(x => x.Id == loanId);
		if(school == null || loan == null) return new NotFoundCommandResult();
		
		school.UpdateLoan(loanId, amount, purpose, provider, interestRate, schedule);
		return new CommandSuccessResult();
	}

	public CommandResult DeleteLoan(int schoolId, int loanId)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		var loan = school?.Loans.FirstOrDefault(x => x.Id == loanId);
		if(school == null || loan == null) return new NotFoundCommandResult();
		
		school.DeleteLoan(loanId);
		return new CommandSuccessResult();
	}

	public CommandResult CreateLease(int schoolId, string leaseTerm, decimal repaymentAmount, decimal interestRate,
		decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		if(school == null) return new NotFoundCommandResult();
		
		school.AddLease(leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets, responsibleForAssets);
		return new CommandSuccessResult();
	}

	public CommandResult UpdateLease(int schoolId, int leaseId, string leaseTerm, decimal repaymentAmount, decimal interestRate,
		decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		var lease = school?.Leases.FirstOrDefault(x => x.Id == leaseId);
		if(school == null || lease == null) return new NotFoundCommandResult();
		
		school.UpdateLease(leaseId, leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets, responsibleForAssets);
		return new CommandSuccessResult();
	}

	public CommandResult DeleteLease(int schoolId, int leaseId)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		var lease = school?.Leases.FirstOrDefault(x => x.Id == leaseId);
		if(school == null || lease == null) return new NotFoundCommandResult();
		
		school.DeleteLease(leaseId);
		return new CommandSuccessResult();
	}

	public CommandResult SetFormTrustDetails(FormTrustDetails formTrustDetails)
	{
		// check the application type allows form trust details to be set
		var validationResult = setformJoinTrustDetailsValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		// if the trust is already set update the fields
		if (FormTrust != null)
		{
			FormTrust.Update(formTrustDetails);

		}
		else
		{
			FormTrust = Trusts.FormTrust.Create(formTrustDetails);
		}

		return new CommandSuccessResult();
	}

	public CommandResult AddTrustKeyPerson(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
	{
		if (this.FormTrust == null)
		{
			throw new InvalidOperationException("Cannot add trust key persons without setting form trust details");
		}

		this.FormTrust.AddTrustKeyPerson(name, dateOfBirth, biography, roles);

		return new CommandSuccessResult();
	}

	public CommandResult UpdateTrustKeyPerson(int keyPersonId, string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
	{
		if (this.FormTrust == null)
		{
			throw new InvalidOperationException("Cannot add trust key persons without setting form trust details");
		}

		this.FormTrust.UpdateTrustKeyPerson(keyPersonId, name, dateOfBirth, biography, roles);

		return new CommandSuccessResult();
	}

	public CommandResult DeleteTrustKeyPerson(int keyPersonId)
	{
		if (this.FormTrust == null)
		{
			throw new InvalidOperationException("Cannot add trust key persons without setting form trust details");
		}

		this.FormTrust.DeleteTrustKeyPerson(keyPersonId);

		return new CommandSuccessResult();
	}

	public CommandResult SetAdditionalDetails(
		int schoolId,
		string trustBenefitDetails, 
		string? ofstedInspectionDetails, 
		string? safeguardingDetails, 
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
		string? furtherInformation)
	{
		var school = _schools.FirstOrDefault(x => x.Id == schoolId);
		if (school == null) return new NotFoundCommandResult();
		school.SetAdditionalDetails(
			trustBenefitDetails,
			ofstedInspectionDetails,
			safeguardingDetails,
			localAuthorityReorganisationDetails,
			localAuthorityClosurePlanDetails,
			dioceseName,
			dioceseFolderIdentifier,
			partOfFederation,
			foundationTrustOrBodyName,
			foundationConsentFolderIdentifier,
			exemptionEndDate,
			mainFeederSchools,
			resolutionConsentFolderIdentifier,
			protectedCharacteristics,
			furtherInformation);
		
		return new CommandSuccessResult();
	}
}

