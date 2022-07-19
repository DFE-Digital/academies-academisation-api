using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;

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
		var contributersEnumerable = contributors.Select(c => new Contributor(c.Key, c.Value));
		_contributors.AddRange(contributersEnumerable);
	}

	public int ApplicationId { get; private set; }
	public ApplicationType ApplicationType { get; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	public void SetIdsOnCreate(int applicationId, int contributorId)
	{
		ApplicationId = applicationId;
		_contributors.Single().Id = contributorId;
	}

	internal static async Task<ConversionApplication> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		var validationResult = await CreateValidator.ValidateAsync(initialContributor);

		if (!validationResult.IsValid) throw new ValidationException(validationResult.ToString());

		return new(applicationType, initialContributor);
	}
}

