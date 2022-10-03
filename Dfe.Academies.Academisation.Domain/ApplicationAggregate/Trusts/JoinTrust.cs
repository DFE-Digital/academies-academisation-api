using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class JoinTrust : IJoinTrust
	{
		public JoinTrust(int id, int ukPrn, string trustName)
		{
			this.Id = id;
			this.UkPRN = ukPrn;
			this.TrustName = trustName;
		}

		public int Id { get; }
		public int UkPRN { get; private set; }
		public string TrustName { get; private set; }

		public static IJoinTrust Create(int ukPrn, string trustName)
		{
			return new JoinTrust(0, ukPrn, trustName);
		}

		public void Update(int ukPrn, string trustName)
		{
			this.UkPRN = ukPrn;
			this.TrustName = trustName;
		}
	}
}
