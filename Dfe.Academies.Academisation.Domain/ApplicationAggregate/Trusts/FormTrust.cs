using System.Collections.ObjectModel;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class FormTrust : DynamicsApplicationEntity, IFormTrust
	{
		protected FormTrust() { }
		private readonly List<TrustKeyPerson> _keyPeople = new();
		private FormTrust(int id, FormTrustDetails trustDetails, IEnumerable<TrustKeyPerson> keyPeople)
		{
			this.Id = id;
			this.TrustDetails = trustDetails;
			this._keyPeople = keyPeople.ToList();
		}

		public FormTrustDetails TrustDetails { get; private set; }
		IReadOnlyCollection<ITrustKeyPerson> IFormTrust.KeyPeople => this._keyPeople.AsReadOnly();
		public IEnumerable<TrustKeyPerson> KeyPeople => this._keyPeople.AsReadOnly();

		public static FormTrust Create(FormTrustDetails trustDetails)
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
