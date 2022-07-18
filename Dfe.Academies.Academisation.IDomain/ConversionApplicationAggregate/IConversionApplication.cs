using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IConversionApplication
{
	ApplicationType ApplicationType { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }
}