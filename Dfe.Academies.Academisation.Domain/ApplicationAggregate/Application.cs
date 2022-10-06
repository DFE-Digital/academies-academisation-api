using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class Application : IApplication
{
	private readonly List<Contributor> _contributors = new();
	private readonly List<School> _schools = new();
	private readonly SubmitApplicationValidator submitValidator = new();
	private readonly UpdateApplicationValidator updateValidator = new();
	private readonly SetJoinTrustDetailsValidator setJoinTrustDetailsValidator = new();

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
		IFormTrust? formTrust)
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
	}

	public int ApplicationId { get; private set; }
	public DateTime CreatedOn { get; }
	public DateTime LastModifiedOn { get; }
	public ApplicationType ApplicationType { get; }
	public ApplicationStatus ApplicationStatus { get; private set; }

	public IFormTrust? FormTrust { get; private set; }
	public IJoinTrust? JoinTrust { get; private set; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	public IReadOnlyCollection<ISchool> Schools => _schools.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		ApplicationId = applicationId;
		_contributors.Single().Id = contributorId;
	}

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
				school.SchoolDetails,
				school.Loans.Select(l => new Loan(l.Key, l.Value))
			));
		}

		return new CommandSuccessResult();
	}

	public CommandResult Submit()
	{
		var validationResult = submitValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		ApplicationStatus = ApplicationStatus.Submitted;

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

	public CommandResult SetJoinTrustDetails(int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained)
	{
		// check the application type allows join trust details to be set
		var validationResult = setJoinTrustDetailsValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		// if the tust is already set update the fields
		if (JoinTrust != null)
		{
			JoinTrust.Update(UKPRN, trustName, changesToTrust, changesToTrustExplained);

		}
		else { 
			JoinTrust = Trusts.JoinTrust.Create(UKPRN, trustName, changesToTrust, changesToTrustExplained); 
		}

		return new CommandSuccessResult();
	}
}

