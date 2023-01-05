using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class TrustKeyPerson : DynamicsKeyPersonEntity, ITrustKeyPerson
	{
		protected TrustKeyPerson() { }
		private readonly List<TrustKeyPersonRole> _roles;
		private TrustKeyPerson(int id, string name, DateTime dateOfBirth, string biography, IEnumerable<TrustKeyPersonRole> roles)
		{
			Id = id;
			Name = name;
			DateOfBirth = dateOfBirth;
			Biography = biography;
			_roles = roles.ToList();
		}

		public int Id { get; private set; }

		public string Name { get; private set; }

		public DateTime DateOfBirth { get; private set; }
		public string Biography { get; private set; }

		IReadOnlyCollection<ITrustKeyPersonRole> ITrustKeyPerson.Roles => _roles.AsReadOnly();
		public IEnumerable<TrustKeyPersonRole> Roles => _roles.AsReadOnly();

		public void Update(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
		{
			this.Name = name;
			this.DateOfBirth = dateOfBirth;
			this.Biography = biography;

			var roleUpdates = roles.Where(x => _roles.Any(r => r.Id == x.Id)).ToList();
			var roleAdds = roles.Where(x => _roles.All(r => r.Id != x.Id)).Cast<TrustKeyPersonRole>().ToList();

			for (int i = _roles.Count - 1; i >= 0; i--)
			{
				var role = _roles[i];
				var roleUpdate = roleUpdates.SingleOrDefault(x => x.Id == role.Id);

				// the roles sent into this method represent all roles for the person
				// if we don't match a role in the list of update then we remove the role
				if (roleUpdate != null)
				{
					role.Update(roleUpdate.Role, roleUpdate.TimeInRole);
				}
				else
				{
					_roles.RemoveAt(i);
				}
			}

			_roles.AddRange(roleAdds);

		}

		public static ITrustKeyPerson Create(string name, DateTime dateOfBirth, string biography, IEnumerable<ITrustKeyPersonRole> roles)
		{
			return new TrustKeyPerson(0, name, dateOfBirth, biography, roles.Cast<TrustKeyPersonRole>());
		}
	}

}
