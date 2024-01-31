using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate
{
	public interface IFormAMatProject
	{
		int Id { get; }
		string ProposedTrustName { get; }

		string ApplicationReference { get;  }
	}
}
