using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate
{
	public class InvoluntaryProject : IInvoluntaryProject
	{
		public ICollection<ISchool> Schools { get; set; }
		public IJoinTrust? JoinTrust { get; set; }
	}
}
