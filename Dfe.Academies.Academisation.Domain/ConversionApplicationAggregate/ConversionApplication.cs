using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplication : IConversionApplication
{
	private readonly List<IContributor> _contributors = new();
	private readonly static CreateConversionApplicationValidator _createValidator = new();

	private ConversionApplication(ApplicationType applicationType, IContributorDetails initialContributor)
	{
		ApplicationType = applicationType;
		_contributors.Add(new Contributor(initialContributor));
	}

	public ApplicationType ApplicationType { get; init; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();

	internal static async Task<ConversionApplication> Create(ApplicationType applicationType, IContributorDetails initialContributor)
	{
		var validationResult = await _createValidator.ValidateAsync(initialContributor);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.ToString());
		}

		return new ConversionApplication(applicationType, initialContributor);
	}
}