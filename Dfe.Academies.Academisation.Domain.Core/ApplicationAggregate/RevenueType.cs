using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public enum RevenueType
	{
		[Description("Surplus")]
		Surplus = 1,
		[Description("Deficit")]
		Deficit = 2
	}
}
