using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;

namespace Dfe.Academies.Academisation.IDomain.UserRoleAggregate
{
	public interface IUserRole
	{
		int Id { get; }
		string RoleId { get; }
		public User? AssignedUser { get; }
		bool IsEnabled { get; }
		void SetAssignedUser(Guid userId, string fullName, string emailAddress);
		void SetRole(RoleId role, DateTime lastModifiedOn, bool isEnabled = true);
	}
}
