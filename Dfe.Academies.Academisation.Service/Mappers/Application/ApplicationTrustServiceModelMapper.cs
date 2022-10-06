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
		internal static ApplicationJoinTustServiceModel? FromDomain(IJoinTrust joinTrust)
		{
			if (joinTrust != null)
			{
				return new(joinTrust.Id, joinTrust.TrustName, joinTrust.UKPrn);
			}

			return default;
		}

		internal static ApplicationFormTrustServiceModel? FromDomain(IFormTrust formTrust)
		{
			if (formTrust != null)
			{
				return new(formTrust.Id, formTrust.ProposedTrustName, formTrust.TrustDetails?.TrustApproverName!);
			}

			return default;
		}


	}
}

