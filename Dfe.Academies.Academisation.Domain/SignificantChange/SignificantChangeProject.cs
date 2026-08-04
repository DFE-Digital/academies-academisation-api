using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public class SignificantChangeProject(SignificantChangeStatus status, int urn, byte tier, string trustName, string trustUkprn, string typeOfSignificantChange, string schoolName) : Entity, IAggregateRoot
	{

		public SignificantChangeStatus Status { get; private set; } = status;
		public int Urn { get; private set; } = urn;
		public string SchoolName { get; private set; } = schoolName;
		public byte Tier { get; private set; } = tier;
		public Guid? AssignedUserId { get; private set; }
		public string? AssignedUserFullName { get; private set; }
		public string? AssignedUserEmailAddress { get; private set; }
		public string TrustName { get; private set; } = trustName;
		public string TrustUkprn { get; private set; } = trustUkprn;
		public string TypeOfSignificantChange { get; private set; } = typeOfSignificantChange;

		public void AssignUser(Guid userId, string userEmail, string userFullName)
		{
			AssignedUserId = userId;
			AssignedUserEmailAddress = userEmail;
			AssignedUserFullName = userFullName;
		}

		public static SignificantChangeProject Create(int urn, byte tier, string trustName, string trustUkprn,
			string route, string schoolName, DateTime createdOn)
		{
			return new SignificantChangeProject(SignificantChangeStatus.PreDecision, urn, tier, trustName, trustUkprn,
				route, schoolName) { CreatedOn = createdOn };
		}
	}
}
