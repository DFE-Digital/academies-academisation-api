using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class TrustKeyPersonRole : ITrustKeyPersonRole
	{
		private TrustKeyPersonRole(int id, KeyPersonRole role, string timeInRole)
		{
			Id = id;
			Role = role;
			TimeInRole = timeInRole;
		}

		public int Id { get; private set; }
		public KeyPersonRole Role { get; private set; }
		public string TimeInRole { get; private set; }
		public void Update(KeyPersonRole role, string timeInRole)
		{
			this.TimeInRole = timeInRole;
			this.Role = role;
		}

		public static ITrustKeyPersonRole Create(KeyPersonRole role, string timeInRole)
		{
			return new TrustKeyPersonRole(0, role,
				timeInRole);
		}

		public static ITrustKeyPersonRole Create(int trustKeyPersonRoleId, KeyPersonRole role, string timeInRole)
		{
			return new TrustKeyPersonRole(trustKeyPersonRoleId, role,
				timeInRole);
		}
	}

}
