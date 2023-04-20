using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public enum KeyPersonRole
	{
		[Description("CEO")]
		CEO = 1,
		[Description("The chair of the trust")]
		Chair = 2,
		[Description("Financial director")]
		FinancialDirector = 3,
		[Description("Trustee")]
		Trustee = 4,
		[Description("Other")]
		Other = 5,
		[Description("Member")]
		Member = 6,
	}
}
