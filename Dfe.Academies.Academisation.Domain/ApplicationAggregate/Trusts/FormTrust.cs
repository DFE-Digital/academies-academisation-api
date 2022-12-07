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
		private readonly List<TrustKeyPerson> _keyPeople;
		private FormTrust(int id, FormTrustDetails trustDetails, IEnumerable<TrustKeyPerson> keyPeople)
		{
			this.Id = id;
			this.TrustDetails = trustDetails;
			this._keyPeople = keyPeople.ToList();
		}

		public FormTrustDetails TrustDetails { get; private set; }
		public ReadOnlyCollection<ITrustKeyPerson> KeyPeople => this._keyPeople.Cast<ITrustKeyPerson>().ToList().AsReadOnly();
		public int Id { get; }

		public static IFormTrust Create(FormTrustDetails trustDetails)
		{
			return new FormTrust(0, trustDetails, new List<TrustKeyPerson>());
		}

		public void Update(FormTrustDetails formTrustDetails)
		{
			TrustDetails = formTrustDetails;
		}

		public void AddTrustKeyPerson(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
		{
			var trustKeyPerson = TrustKeyPerson.Create(name, dateOfBirth, biography, roles);

			this._keyPeople.Add(trustKeyPerson as TrustKeyPerson);
		}

		public void UpdateTrustKeyPerson(int keyPersonId, string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
		{
			var keyPerson = this._keyPeople.FirstOrDefault(x => x.Id == keyPersonId);

			keyPerson?.Update(name, dateOfBirth, biography, roles);
		}

		public void DeleteTrustKeyPerson(int keyPersonId)
		{
			var keyPerson = this._keyPeople.FirstOrDefault(x => x.Id == keyPersonId);
			if (keyPerson == null) return;
			this._keyPeople.Remove(keyPerson);
		}
	}
}
