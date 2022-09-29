using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public abstract class Trust : ITrust
	{
		public int Id { get; protected set; }

		public int UkPRN { get; protected set; }

		public string TrustName { get; protected set; }
	}
}
