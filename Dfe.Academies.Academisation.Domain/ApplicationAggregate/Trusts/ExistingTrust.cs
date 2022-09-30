using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class ExistingTrust : Trust, IExistingTrust
	{
		public ExistingTrust(int id, int ukPrn, string trustName)
		{
			this.Id = id;
			this.UkPRN = ukPrn;
			this.TrustName = trustName;
		}

		public static IExistingTrust Create(int ukPrn, string trustName)
		{

			return new ExistingTrust(0, ukPrn, trustName);
		}
	}
}
