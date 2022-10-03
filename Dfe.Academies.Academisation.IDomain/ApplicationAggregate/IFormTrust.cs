using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface IFormTrust
	{
		public int Id { get; }

		public string ProposedTrustName { get; }

		public FormTrustDetails TrustDetails { get; }
	}
}
