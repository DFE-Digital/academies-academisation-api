using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface IJoinTrust
	{
		public int Id { get; }

		public string TrustName { get; }

		public int UKPRN { get; }

		public bool? ChangesToTrust { get; }

		public string? ChangesToTrustExplained { get; }

		public void Update(int UKPRN, string trustName, bool? changesToTrust, string? changesToTrustExplained);
	}
}
