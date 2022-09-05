using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ApplicationAggregate;

internal static class LegacyKeyPersonServiceModelMapper
{
	internal static ICollection<LegacyKeyPersonServiceModel> MapToServiceModels(this IApplication application)
	{
		if (application.ApplicationType == ApplicationType.JoinAMat)
		{
			// Key Persons only applies when forming a trust, so for JoinAMat it will be an empty collection
			return new List<LegacyKeyPersonServiceModel>();
		}

		// For FormAMat and FormASat it would be necessary to map this from the NewTrust object
		// (but it's probably not worth implementing them in this legacy service model).
		// If we do, then this will probably be mapped from application.NewTrust.KeyPersons or something like
		throw new NotImplementedException("Only JoinAMat has been implemented");
	}
}
