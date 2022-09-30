using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class ApplicationTrustServiceModelMapper
	{
		internal static ApplicationTrustServiceModel? FromDomain<T>(T trust)
		{
			var existingTrust = trust as IExistingTrust;

			if (existingTrust != null)
			{
				return new(existingTrust.Id, existingTrust.UkPRN, existingTrust.TrustName, null);
			}

			var newTrust = trust as INewTrust;

			if (newTrust != null)
			{
				return new(newTrust.Id, newTrust.UkPRN, newTrust.TrustName, newTrust.TrustDetails.TrustApproverName);
			}

			return default;
		}


	}
}

