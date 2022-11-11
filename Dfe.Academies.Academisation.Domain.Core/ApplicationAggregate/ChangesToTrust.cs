using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public enum ChangesToTrust
	{
		[Description("Yes")]
		Yes = 1,
		[Description("No")]
		No = 2,
		[Description("Unknown")]
		Unknown = 3,
	}
}
