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

	}
}
