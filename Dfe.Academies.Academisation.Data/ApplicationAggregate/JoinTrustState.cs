using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationJoinTrust")]
	public class JoinTrustState : BaseEntity
	{
		public Guid? DynamicsApplicationId { get; set; }
		public int UKPRN { get; set; }
		public string TrustName { get; set; } = null!;

		public ChangesToTrust? ChangesToTrust { get; set; }
		public string? ChangesToTrustExplained { get; set; }
		public bool? ChangesToLaGovernance { get; set; }
		public string? ChangesToLaGovernanceExplained { get; set;  }
	}
}
