using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplication : IConversionApplication
{
	private readonly List<Contributor> _contributors = new();
	private readonly List<ApplicationSchool> _schools = new();
	private static readonly CreateConversionApplicationValidator CreateValidator = new();
	private static readonly SubmitConversionApplicationValidator SubmitValidator = new();

	private ConversionApplication(ApplicationType applicationType, ContributorDetails initialContributor)
	{
		ApplicationStatus = ApplicationStatus.InProgress;
		ApplicationType = applicationType;
		_contributors.Add(new(initialContributor));
	}

	public ConversionApplication(int applicationId, ApplicationType applicationType, ApplicationStatus applicationStatus,
		Dictionary<int, ContributorDetails> contributors,
		Dictionary<int, ApplicationSchoolDetails> schools)
	{
		ApplicationId = applicationId;
		ApplicationType = applicationType;
		ApplicationStatus = applicationStatus;
		_contributors = contributors.Select(c => new Contributor(c.Key, c.Value)).ToList();
		_schools = schools.Select(s => new ApplicationSchool(s.Key, s.Value)).ToList();
	}

	public int ApplicationId { get; private set; }
	public ApplicationType ApplicationType { get; }
	public ApplicationStatus ApplicationStatus { get; private set; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	public IReadOnlyCollection<IApplicationSchool> Schools => _schools.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		ApplicationId = applicationId;
		_contributors.Single().Id = contributorId;
	}

	public CommandResult Submit()
	{
		var validationResult = SubmitValidator.Validate(this);

		if (!validationResult.IsValid)
		{
			return new CommandValidationErrorResult(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		ApplicationStatus = ApplicationStatus.Submitted;

		return new CommandSuccessResult();
	}

	internal static CreateResult<IConversionApplication> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		var validationResult = CreateValidator.Validate(initialContributor);

		if (!validationResult.IsValid)
		{
			return new CreateValidationErrorResult<IConversionApplication>(
				validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
		}

		return new CreateSuccessResult<IConversionApplication>(new ConversionApplication(applicationType, initialContributor));
	}
}

