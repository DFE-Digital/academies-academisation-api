using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ITrustKeyPerson
	{
		int Id { get;  }
		KeyPersonRole Role { get; }
		string TimeInRole { get; }
		string FirstName { get; }
		string Surname { get; }
		string? ContactEmailAddress { get; }
		DateTime? DateOfBirth { get;  }
		string Biography { get; }
		void Update(string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography);
	}
}
