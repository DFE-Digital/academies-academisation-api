using System.Security.Cryptography.X509Certificates;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.Domain.Validations;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class Application : DynamicsApplicationEntity, IApplication, IAggregateRoot
{
	private readonly List<Contributor> _contributors = new();
	private readonly List<School> _schools = new();
	private readonly SubmitApplicationValidator submitValidator = new();
	private readonly UpdateApplicationValidator updateValidator = new();
	private readonly SetJoinTrustDetailsValidator setJoinTrustDetailsValidator = new();
	private readonly SetFormTrustDetailsValidator setformJoinTrustDetailsValidator = new();

	protected Application() { }

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
		JoinTrust? joinTrust,
		FormTrust? formTrust,
		DateTime? applicationSubmittedOn = null,
		string? applicationReference = null)
	{
		Id = applicationId;
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

	public int ApplicationId { get { return Id; } }
	public ApplicationType ApplicationType { get; }
	public ApplicationStatus ApplicationStatus { get; private set; }
	public DateTime? ApplicationSubmittedDate { get; private set; }

	public FormTrust? FormTrust { get; private set; }
	public JoinTrust? JoinTrust { get; private set; }

	IFormTrust? IApplication.FormTrust => FormTrust;
	IJoinTrust? IApplication.JoinTrust => JoinTrust;

	//these need to be doubled up for EF config until we merge the IDomain and Domain projects
	public IEnumerable<Contributor> Contributors => _contributors.AsReadOnly();
	public IEnumerable<School> Schools => _schools.AsReadOnly();

	IReadOnlyCollection<IContributor> IApplication.Contributors => _contributors.AsReadOnly();

	IReadOnlyCollection<ISchool> IApplication.Schools => _schools.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		Id = applicationId;
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

		// Update Contributors
		for (int i = _contributors.Count -1; i >= 0; i--)
		{
			var contributor = _contributors[i];

			// if it has an update in the list
			if (contributors.Any(x => x.Key == contributor.Id))
			{
				var contributorUpdate = contributors.SingleOrDefault(x => x.Key == contributor.Id);
				contributor.Update(contributorUpdate.Value);

			}

			// if it isn't in the list remove it
			if (contributors.All(x => x.Key != contributor.Id))
			{
				_contributors.Remove(contributor);
			}
		}

		// add ones that are new
		var contributorsToAdd = contributors.Where(x => _contributors.All(c => c.Id != x.Key));
		_contributors.AddRange(contributorsToAdd.Select(x => new Contributor(
					0,
					x.Value
					)));

		// Update Schools
		for (int i = _schools.Count - 1; i >= 0; i--)
		{
			var school = _schools[i];

			// if it has an update in the list
			if (schools.Any(x => x.Id == school.Id))
			{
				var schoolUpdate = schools.SingleOrDefault(x => x.Id == school.Id);
				school.Update(schoolUpdate);

			}

			// if it isn't in the list remove it
			if (schools.All(x => x.Id != school.Id))
			{
				_schools.Remove(school);
			}
		}

		// add ones that are new
		var schoolsToAdd = schools.Where(x => _schools.All(c => c.Id != x.Id));
		_schools.AddRange(schoolsToAdd.Select(school =>
			new School(0,
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
				school.Loans.Select(l => new Loan(0, l.Value.Amount!.Value, l.Value.Purpose!, l.Value.Provider!, l.Value.InterestRate!.Value, l.Value.Schedule!)),
				school.Leases.Select(l => new Lease(0, l.Value.leaseTerm, l.Value.repaymentAmount, l.Value.interestRate, l.Value.paymentsToDate, l.Value.purpose, l.Value.valueOfAssets, l.Value.responsibleForAssets)),
				school.HasLoans, school.HasLeases)));

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

	internal static CreateResult Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		var validationResult = new CreateApplicationValidator().Validate(initialContributor);

		if (!validationResult.IsValid)
		{
			return new CreateValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		return new CreateSuccessResult<Application>(new Application(applicationType, initialContributor));
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
			JoinTrust = JoinTrust.Create(UKPRN, trustName, changesToTrust, changesToTrustExplained, changesToLaGovernance, changesToLaGovernanceExplained); 
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
			FormTrust = FormTrust.Create(formTrustDetails);
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

	public CommandResult DeleteSchool(int urn)
	{
		var school = _schools.FirstOrDefault(x => x.Details.Urn == urn);
		if (school == null)  return new NotFoundCommandResult();
		_schools.Remove(school);

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

