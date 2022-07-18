using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplication : IConversionApplication
{
	private readonly List<IContributor> _contributors = new();
	private static readonly CreateConversionApplicationValidator CreateValidator = new();

	private ConversionApplication(ApplicationType applicationType, ContributorDetails initialContributor)
	{
		ApplicationType = applicationType;
		_contributors.Add(new Contributor(initialContributor));
	}

	public int ApplicationId { get; set; }
	public ApplicationType ApplicationType { get; }

	public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();
	
	public void SetApplicationId(int applicationId)
	{
		ApplicationId = applicationId;
	}

	internal static async Task<ConversionApplication> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		var validationResult = await CreateValidator.ValidateAsync(initialContributor);

		if (!validationResult.IsValid) throw new ValidationException(validationResult.ToString());

		return new(applicationType, initialContributor);
	}
}

