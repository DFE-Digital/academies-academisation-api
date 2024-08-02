using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public class ProjectGroup : Entity, IProjectGroup, IAggregateRoot
	{
		public string TrustName { private set; get; } = string.Empty;
		public string TrustReference { private set; get; } = string.Empty;
		public string TrustUkprn { private set; get; } = string.Empty;

		public User? AssignedUser { private set; get; }

		public string? ReferenceNumber { private set; get; }

		public void SetAssignedUser(Guid userId, string fullName, string emailAddress)
		{
			AssignedUser = new User(userId, fullName, emailAddress);
		}

		public void SetProjectGroup(string trustReference, DateTime lastModifiedOnUtc)
		{
			TrustReference = trustReference;
			LastModifiedOn = lastModifiedOnUtc;
		}

		public static ProjectGroup Create(string trustReference, string trustUkprn, string trustName, DateTime createdOn)
		{
			return new ProjectGroup(trustReference, trustUkprn, trustName, createdOn);
		}

		private ProjectGroup(string trustReference, string trustUkprn, string trustName, DateTime createdOn)
		{
			TrustReference = trustReference;
			TrustUkprn = trustUkprn;
			TrustName = trustName;
			CreatedOn = createdOn;
		}

		public void SetProjectReference(int id)
		{
			// Convert the id to string and pad it with zeros to ensure it is 8 characters long
			string paddedId = id.ToString().PadLeft(8, '0');

			// Set the ReferenceNumber property with the formatted string
			ReferenceNumber = $"GRP_{paddedId}";
		}
	}
}
