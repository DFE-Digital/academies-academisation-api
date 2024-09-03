
namespace Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate
{
	public enum RoleCapability
	{
		ConversionAdministrator,
		TransferAdministrator,
		FromAMatAdministrator,
		GroupAdministrator,
		DeleteProject,
		UserAdministrator
	}
	public class Roles
	{
		public static List<RoleCapability> GetRoleCapabilities(RoleId roleId)
		{
			return roleId switch
			{
				RoleId.SuperAdmin => SuperAdminRoleCapabilities(),
				RoleId.Manager => ManagerRoleCapabilities(),
				_ => StandardRoleCapabilities(),
			};
		}
		private static List<RoleCapability> StandardRoleCapabilities()
		{
			return [
				RoleCapability.ConversionAdministrator,
				RoleCapability.TransferAdministrator,
				RoleCapability.GroupAdministrator,
				RoleCapability.FromAMatAdministrator,
			];
		}

		private static List<RoleCapability> ManagerRoleCapabilities()
		{
			return StandardRoleCapabilities().Concat([
				RoleCapability.DeleteProject
			]).Distinct().ToList();
		}

		private static List<RoleCapability> SuperAdminRoleCapabilities()
		{
			return StandardRoleCapabilities().Concat(ManagerRoleCapabilities()).Concat([
				RoleCapability.UserAdministrator
			]).Distinct().ToList();
		}
	}
}
