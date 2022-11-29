using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class TrustKeyPerson : ITrustKeyPerson
	{
		private TrustKeyPerson(int id, string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			Id = id;
			FirstName = firstName;
			Surname = surname;
			DateOfBirth = dateOfBirth;
			ContactEmailAddress = contactEmailAddress;
			Role = role;
			TimeInRole = timeInRole;
			Biography = biography;
		}

		public int Id { get; private set; }
		public KeyPersonRole Role { get; private set; }
		public string TimeInRole { get; private set; }
		public string FirstName { get; private set; }

		public string Surname { get; private set; }

		public string? ContactEmailAddress { get; private set; }
		public DateTime? DateOfBirth { get; private set; }
		public string Biography { get; private set; }

		public void Update(string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role,
			string timeInRole, string biography)
		{
			this.FirstName = firstName;
			this.Surname = surname;
			this.DateOfBirth = dateOfBirth;
			this.ContactEmailAddress = contactEmailAddress;
			this.Role = role;
			this.TimeInRole = timeInRole;
			this.Biography = biography;
		}

		public static ITrustKeyPerson Create(string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			return new TrustKeyPerson(0, firstName, surname, dateOfBirth, contactEmailAddress, role,
				timeInRole, biography);
		}
	}

}
