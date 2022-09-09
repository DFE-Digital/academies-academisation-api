using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public enum PayFundsTo
{
	[Description("To the school")]
	School = 1,
	[Description("To the trust the school is joining")]
	Trust = 2
}
