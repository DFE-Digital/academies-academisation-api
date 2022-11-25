using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ITrustKeyPerson
	{
		int Id { get; set; }
		int TrustId { get; set; }
		KeyPersonRole Role { get; set; }
		string TimeInRole { get; set; }
		int PersonId { get; set; }
		string FirstName { get; set; }
		string Surname { get; set; }
		string? ContactEmailAddress { get; set; }
		DateTime? DateOfBirth { get; set; }
		string Biography { get; set; }
		void Update(string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography);
	}
}
