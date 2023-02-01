using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface IJoinTrust
	{
		public int Id { get; }

		public string TrustName { get; }

		public string TrustReference { get; }

		public int UKPRN { get; }

		public ChangesToTrust? ChangesToTrust { get; }

		public string? ChangesToTrustExplained { get; }

		public bool? ChangesToLaGovernance { get; }

		public string? ChangesToLaGovernanceExplained { get; }

		public void Update(int UKPRN, string trustName, string trustReference, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained);
	}
}
