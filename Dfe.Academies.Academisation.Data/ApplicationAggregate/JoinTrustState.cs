using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationJoinTrust")]
	public class JoinTrustState : BaseEntity
	{
		public int UKPRN { get; set; }
		public string TrustName { get; set; } = null!;

		public bool? ChangesToTrust { get; set; }
		public string? ChangesToTrustExplained { get; set; }
		public bool? ChangesToLaGovernance { get; set; }
		public string? ChangesToLaGovernanceExplained { get; set;  }
	}
}
