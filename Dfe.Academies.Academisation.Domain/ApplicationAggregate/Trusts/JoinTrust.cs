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
		private JoinTrust(int id, int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained)
		{
			this.Id = id;
			this.UKPRN = UKPRN;
			this.TrustName = trustName;
			this.ChangesToTrust = changesToTrust;
			this.ChangesToTrustExplained = changesToTrustExplained;
		}

		public int Id { get; }

		public int UKPRN { get; private set; }

		public string TrustName { get; private set; }

		public bool? ChangesToTrust { get; private set; }

		public string? ChangesToTrustExplained { get; private set; }

		public static IJoinTrust Create(int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained)
		{
			return new JoinTrust(0, UKPRN, trustName, changesToTrust, changesToTrustExplained);
		}

		public void Update(int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained)
		{
			this.UKPRN = UKPRN;
			this.TrustName = trustName;
			this.ChangesToTrust = changesToTrust;
			this.ChangesToTrustExplained = changesToTrustExplained;
		}
	}
}
