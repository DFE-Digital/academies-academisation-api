
namespace Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate
{
	public enum RoleCapability
	{
		DeleteTransferProject,
		DeleteGroupProject,
		DeleteConversionProject,
		UserAdministrator
	}
	public static class Roles
	{
		public static List<RoleCapability> GetRoleCapabilities(string roleId)
		{
			return roleId == RoleId.SuperAdmin.GetStringValue()
				? SuperAdminRoleCapabilities() : (roleId == RoleId.Manager.GetStringValue()
				? ManagerRoleCapabilities() : StandardRoleCapabilities());
		}
		private static List<RoleCapability> StandardRoleCapabilities()
		{
			return [
				RoleCapability.DeleteGroupProject
			];
		}

		private static List<RoleCapability> ManagerRoleCapabilities()
		{
			return StandardRoleCapabilities().Concat([
				RoleCapability.DeleteConversionProject,
				RoleCapability.DeleteTransferProject
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
