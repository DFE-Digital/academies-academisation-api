using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate
{
	public interface IProjectGroup
	{
		int Id { get; }

		string TrustReference{ get; }
		string TrustName{ get; }
		string TrustUkprn{ get; }

		User? AssignedUser { get; }

		DateTime CreatedOn { get; }

		string? ReferenceNumber { get; }

		void SetAssignedUser(Guid userId, string fullName, string emailAddress);

		void SetProjectReference(int id);

		void SetProjectGroup(string trustReference, DateTime lastModifiedOnUtc);
	}
}
