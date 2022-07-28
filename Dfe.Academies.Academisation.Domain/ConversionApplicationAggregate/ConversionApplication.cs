using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Core;
using System.Linq;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplication : IConversionApplication
{
	private readonly List<Contributor> _contributors = new();
	private static readonly CreateConversionApplicationValidator CreateValidator = new();
	private static readonly SubmitConversionApplicationValidator SubmitValidator = new();

	private ConversionApplication(ApplicationType applicationType, ContributorDetails initialContributor)
	{
		ApplicationStatus = ApplicationStatus.InProgress;
		ApplicationType = applicationType;
		_contributors.Add(new(initialContributor));
	}
	public ConversionApplication(int applicationId, ApplicationType applicationType, Dictionary<int, ContributorDetails> contributors, ApplicationStatus applicationStatus)
	{
		ApplicationType = applicationType;
		ApplicationId = applicationId;
		ApplicationStatus = applicationStatus;
		var contributorsEnumerable = contributors.Select(c => new Contributor(c.Key, c.Value));
		_contributors.AddRange(contributorsEnumerable);
	}

	public int ApplicationId { get; private set; }
	public ApplicationType ApplicationType { get; }
	public ApplicationStatus ApplicationStatus { get; private set; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

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

