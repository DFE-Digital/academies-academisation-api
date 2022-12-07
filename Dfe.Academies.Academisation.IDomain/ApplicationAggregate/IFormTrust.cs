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

		public ReadOnlyCollection<ITrustKeyPerson> KeyPeople { get; }

		void Update(FormTrustDetails formTrustDetails);

		void AddTrustKeyPerson(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);

		void UpdateTrustKeyPerson(int keyPersonId, string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);

		void DeleteTrustKeyPerson(int keyPersonId);
	}
}
