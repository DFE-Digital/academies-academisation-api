using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.UserRole
{
	public class UserRoleModel(RoleId roleId, string fullName, string email)
	{
		public RoleId RoleId { get; set; } = roleId;
		public string FullName { get; set; } = fullName;
		public string Email { get; set; } = email;
	}
}
