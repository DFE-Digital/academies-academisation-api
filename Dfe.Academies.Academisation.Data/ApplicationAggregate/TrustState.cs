using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationTrust")]
	public class TrustState : BaseEntity
	{
		public int UKPrn { get; set; }
		public string TrustName { get; set; } = null!;

		public string? TrustApproverName { get; set; } = null!;

		public static TrustState MapFromDomain(ITrust trust)
		{
			if (typeof(ExistingTrust) == trust.GetType())
			{
				var existingTrust = trust as ExistingTrust;
				if (existingTrust != null)
				{
					return new()
					{
						Id = existingTrust.Id,
						TrustName = existingTrust.TrustName,
						UKPrn = existingTrust.UkPRN
					};
				}
			}

			// if it isn't an existing trust then must be new

			var newTrust = trust as NewTrust;

			if (newTrust != null)
			{
				return new()
				{
					Id = newTrust.Id,
					TrustName = newTrust.TrustName,
					UKPrn = newTrust.UkPRN,
					TrustApproverName = newTrust.TrustDetails?.TrustApproverName
				};
			}


			return null!;

		}

		public static ITrust MapToDomain(ApplicationType applicationType, TrustState trust)
		{
			if (applicationType == ApplicationType.JoinAMat)
			{
				// if it is join a Mat then must be existing trust
				return new ExistingTrust(trust.Id, trust.UKPrn, trust.TrustName);
			}

			return new NewTrust(trust.Id, trust.UKPrn, trust.TrustName, new TrustDetails(trust.TrustApproverName));
		}
	}
}
