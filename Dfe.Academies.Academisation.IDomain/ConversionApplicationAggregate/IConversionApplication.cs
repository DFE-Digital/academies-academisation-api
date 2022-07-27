using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IConversionApplication
{
	int ApplicationId { get; }
	ApplicationType ApplicationType { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }

	void SetIdsOnCreate(int applicationId, int conversionId);
}
