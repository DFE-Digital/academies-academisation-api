using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public class SignificantChangeProject : Entity, IAggregateRoot
	{
		
		public SignificantChangeStatus Status { get; private set; }
		public int Urn { get; private set; }
		public byte Tier { get; private set; }
		public Guid? AssignedUserId { get; private set; }
		public string? AssignedUserFullName { get; private set; }
		public string? AssignedUserEmailAddress { get; private set; }
		public string TrustName { get; private set; }
		public string TrustUkprn { get; private set; }
		public string TypeOfSignificantChange { get; private set; }

		public SignificantChangeProject(SignificantChangeStatus status, int urn, byte tier, string trustName, string trustUkprn, string typeOfSignificantChange)
		{
			Status = status;
			Urn = urn;
			Tier = tier;
			TrustName = trustName;
			TrustUkprn = trustUkprn;
			TypeOfSignificantChange = typeOfSignificantChange;
		}


		public void AssignUser(Guid userId, string userEmail, string userFullName)
		{
			AssignedUserId = userId;
			AssignedUserEmailAddress = userEmail;
			AssignedUserFullName = userFullName;
		}

		public static SignificantChangeProject Create(int urn, byte tier, string trustName, string trustUkprn,
			string route, DateTime createdOn)
		{
			return new SignificantChangeProject(SignificantChangeStatus.InProgress, urn, tier, trustName, trustUkprn,
				route) { CreatedOn = createdOn };
		}
	}
}
