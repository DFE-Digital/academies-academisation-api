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
			// Key Persons only applies when forming a trust
			return new List<LegacyKeyPersonServiceModel>();
		}

		// For FormAMat and FormASat it would be necessary to map this from the NewTrust object
		// (but it's probably not worth implementing them in this legacy service model)
		throw new NotImplementedException();
	}
}
