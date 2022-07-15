using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IConversionApplication
{
	int ApplicationId { get; }
	ApplicationType ApplicationType { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }

	void SetApplicationId(int applicationId);
}
