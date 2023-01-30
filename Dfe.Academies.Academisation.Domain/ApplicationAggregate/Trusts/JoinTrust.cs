using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class JoinTrust : DynamicsApplicationEntity, IJoinTrust
	{
		protected JoinTrust() { }
		private JoinTrust(int id, int UKPRN, string trustName, string trustReference, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained)
		{
			this.Id = id;
			this.UKPRN = UKPRN;
			this.TrustName = trustName;
			this.TrustReference = trustReference;
			this.ChangesToTrust = changesToTrust;
			this.ChangesToTrustExplained = changesToTrustExplained;
			this.ChangesToLaGovernance = changesToLaGovernance;
			this.ChangesToLaGovernanceExplained = changesToLaGovernanceExplained;
		}

		public int UKPRN { get; private set; }

		public string TrustName { get; private set; }

		public string TrustReference { get; private set; }

		public ChangesToTrust? ChangesToTrust { get; private set; }

		public string? ChangesToTrustExplained { get; private set; }

		public bool? ChangesToLaGovernance { get; private set; }

		public string? ChangesToLaGovernanceExplained { get; private set; }

		public static JoinTrust Create(int UKPRN, string trustName, string trustReference, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained)
		{
			return new JoinTrust(0, UKPRN, trustName, trustReference, changesToTrust, changesToTrustExplained, changesToLaGovernance, changesToLaGovernanceExplained);
		}

		public void Update(int UKPRN, string trustName, string trustReference, ChangesToTrust? changesToTrust, string? changesToTrustExplained, bool? changesToLaGovernance, string? changesToLaGovernanceExplained)
		{
			this.UKPRN = UKPRN;
			this.TrustName = trustName;
			this.TrustReference = trustReference;
			this.ChangesToTrust = changesToTrust;
			this.ChangesToTrustExplained = changesToTrustExplained;
			this.ChangesToLaGovernance = changesToLaGovernance;
			this.ChangesToLaGovernanceExplained = changesToLaGovernanceExplained;
		}
	}
}
