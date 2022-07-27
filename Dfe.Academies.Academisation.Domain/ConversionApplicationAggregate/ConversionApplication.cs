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

	private ConversionApplication(ApplicationType applicationType, ContributorDetails initialContributor)
	{
		ApplicationType = applicationType;
		_contributors.Add(new(initialContributor));
	}
	public ConversionApplication(int applicationId, ApplicationType applicationType, Dictionary<int, ContributorDetails> contributors)
	{
		ApplicationType = applicationType;
		ApplicationId = applicationId;
		var contributorsEnumerable = contributors.Select(c => new Contributor(c.Key, c.Value));
		_contributors.AddRange(contributorsEnumerable);
	}

	public int ApplicationId { get; private set; }
	public ApplicationType ApplicationType { get; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		ApplicationId = applicationId;
		_contributors.Single().Id = contributorId;
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

