using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate
{
	public interface IInvoluntaryProject
	{
		public ICollection<ISchool> Schools { get; set; }
		public IJoinTrust? JoinTrust { get; set; }
	}
}
