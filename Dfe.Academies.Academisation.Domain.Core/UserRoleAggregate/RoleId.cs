using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate
{
	public enum RoleId
	{
		[Description("SuperAdmin")]
		SuperAdmin = 1,
		[Description("Manager")]
		Manager = 2,
		[Description("Standard")]
		Standard = 3
	}
}
