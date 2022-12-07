using System.Collections.ObjectModel;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ITrustKeyPerson
	{
		int Id { get;  }
		string Name { get; }
		DateTime DateOfBirth { get;  }
		string Biography { get; }

		public ReadOnlyCollection<ITrustKeyPersonRole> Roles { get; }

		void Update(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);
	}
}
