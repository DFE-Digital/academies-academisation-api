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
	public static class RoleIdExtensions
	{
		public static string GetStringValue(this RoleId roleId)
		{
			return roleId switch
			{ 
				RoleId.Manager => "71eaea6e-44e2-45b4-bc1b-7542b38200b3",
				RoleId.SuperAdmin => "f61975e2-8669-4524-89e1-e970ca792f03",
				_ => "e9f6ef60-0b4d-439a-ba98-52d11c4d6737"
			};
		}
	}
}
