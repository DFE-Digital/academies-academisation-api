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
		private List<ITrustKeyPerson> trustKeyPeople;
		private FormTrust(int id, FormTrustDetails trustDetails)
		{
			this.Id = id;
			this.TrustDetails = trustDetails;
			trustKeyPeople = new List<ITrustKeyPerson>();
		}

		public FormTrustDetails TrustDetails { get; private set; }
		public ReadOnlyCollection<ITrustKeyPerson> TrustKeyPeople { get { return this.trustKeyPeople.AsReadOnly(); } }
		public int Id { get; }

		public static IFormTrust Create(FormTrustDetails trustDetails)
		{
			return new FormTrust(0, trustDetails);
		}

		public void Update(FormTrustDetails formTrustDetails)
		{
			TrustDetails = formTrustDetails;
		}

		public void AddTrustKeyPerson(string firstName, string surname, DateTime? dateOfBirth,
			string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			var trustKeyPerson = TrustKeyPerson.Create(this.Id, firstName, surname, dateOfBirth,
				contactEmailAddress, role, timeInRole, biography);

			this.trustKeyPeople.Add(trustKeyPerson);
		}

		public void UpdateTrustKeyPerson(int keyPersonId, string firstName, string surname, DateTime? dateOfBirth,
			string? contactEmailAddress, KeyPersonRole role, string timeInRole, string biography)
		{
			var keyPerson = this.trustKeyPeople.FirstOrDefault(x => x.Id == keyPersonId);

			keyPerson?.Update(firstName, surname, dateOfBirth,
				contactEmailAddress, role, timeInRole, biography);
		}

		public void DeleteTrustKeyPerson(int keyPersonId)
		{
			var keyPerson = this.trustKeyPeople.FirstOrDefault(x => x.Id == keyPersonId);
			if (keyPerson == null) return;
			this.trustKeyPeople.Remove(keyPerson);
		}
	}
}
