namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ITrustKeyPerson
	{
		int Id { get;  }
		string Name { get; }
		DateTime DateOfBirth { get;  }
		string Biography { get; }

		public IReadOnlyCollection<ITrustKeyPersonRole> Roles { get; }

		void Update(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles);
	}
}
