using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface IFormTrust
	{
		public int Id { get; }

		public FormTrustDetails TrustDetails { get; }

		public ReadOnlyCollection<ITrustKeyPerson> TrustKeyPeople { get; }

		void Update(FormTrustDetails formTrustDetails);

		void AddTrustKeyPerson(string firstName, string surname, DateTime? dateOfBirth,
			string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography);

		void UpdateTrustKeyPerson(int keyPersonId, string firstName, string surname, DateTime? dateOfBirth, string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography);
		void DeleteTrustKeyPerson(int keyPersonId);
	}
}
