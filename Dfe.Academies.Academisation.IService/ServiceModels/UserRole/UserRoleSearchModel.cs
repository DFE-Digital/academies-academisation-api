
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.UserRole
{
	public class UserRoleSearchModel(int page, int count, string? searchTerm, RoleId? roleId)
	{
		public int Page { get; set; } = page;
		public int Count { get; set; } = count;
		public string? searchTerm { get; set; } = searchTerm;
		public RoleId? RoleId { get; set; } = roleId;
	}
}
