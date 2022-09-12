using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public enum EqualityImpact
	{
		[Description("Considered unlikely")]
		ConsideredUnlikely,
		[Description("Considered will not")]
		ConsideredWillNot,
		[Description("Not considered")]
		NotConsidered
	}
}
