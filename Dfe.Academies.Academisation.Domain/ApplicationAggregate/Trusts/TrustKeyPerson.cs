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
		private TrustKeyPerson(int id, int trustId, string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			Id = id;
			TrustId = trustId;
			FirstName = firstName;
			Surname = surname;
			DateOfBirth = dateOfBirth;
			ContactEmailAddress = contactEmailAddress;
			Role = role;
			TimeInRole = timeInRole;
			Biography = biography;
		}

		public int Id { get; set; }
		public int TrustId { get; set; }
		public KeyPersonRole Role { get; set; }
		public string TimeInRole { get; set; }
		public int PersonId { get; set; }
		public string FirstName { get; set; }

		public string Surname { get; set; }

		public string? ContactEmailAddress { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Biography { get; set; }

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

		public static ITrustKeyPerson Create(int trustId, string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			return new TrustKeyPerson(0, trustId, firstName, surname, dateOfBirth, contactEmailAddress, role,
				timeInRole, biography);
		}
	}

}
