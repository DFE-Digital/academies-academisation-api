using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class FormTrust : IFormTrust
	{
		private readonly List<TrustKeyPerson> keyPeople;
		private FormTrust(int id, FormTrustDetails trustDetails, IEnumerable<TrustKeyPerson> keyPeople)
		{
			this.Id = id;
			this.TrustDetails = trustDetails;
			this.keyPeople = keyPeople.ToList();
		}

		public FormTrustDetails TrustDetails { get; private set; }
		public ReadOnlyCollection<ITrustKeyPerson> KeyPeople => this.keyPeople.Cast<ITrustKeyPerson>().ToList().AsReadOnly();
		public int Id { get; }

		public static IFormTrust Create(FormTrustDetails trustDetails)
		{
			return new FormTrust(0, trustDetails, new List<TrustKeyPerson>());
		}

		public void Update(FormTrustDetails formTrustDetails)
		{
			TrustDetails = formTrustDetails;
		}

		public void AddTrustKeyPerson(string firstName, string surname, DateTime? dateOfBirth,
			string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			var trustKeyPerson = TrustKeyPerson.Create(firstName, surname, dateOfBirth,
				contactEmailAddress, role, timeInRole, biography);

			this.keyPeople.Add(trustKeyPerson as TrustKeyPerson);
		}

		public void UpdateTrustKeyPerson(int keyPersonId, string firstName, string surname, DateTime? dateOfBirth,
			string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			var keyPerson = this.keyPeople.FirstOrDefault(x => x.Id == keyPersonId);

			keyPerson?.Update(firstName, surname, dateOfBirth,
				contactEmailAddress, role, timeInRole, biography);
		}

		public void DeleteTrustKeyPerson(int keyPersonId)
		{
			var keyPerson = this.keyPeople.FirstOrDefault(x => x.Id == keyPersonId);
			if (keyPerson == null) return;
			this.keyPeople.Remove(keyPerson);
		}
	}
}
