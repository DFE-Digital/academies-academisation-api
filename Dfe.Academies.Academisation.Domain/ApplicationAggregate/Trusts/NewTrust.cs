using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class NewTrust : Trust, ITrust
	{
		public NewTrust(int id, int ukPrn, string trustName, TrustDetails trustDetails)
		{
			this.Id = id;
			this.UkPRN = ukPrn;
			this.TrustName = trustName;
			this.TrustDetails = trustDetails;
		}

		public TrustDetails? TrustDetails { get; protected set; }

		public static Trust Create(int ukPrn, string trustName, TrustDetails trustDetails) {

			return new NewTrust(0, ukPrn, trustName, trustDetails);
		}
	}
}
