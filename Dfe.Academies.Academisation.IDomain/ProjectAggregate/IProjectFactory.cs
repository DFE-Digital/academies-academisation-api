using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProjectFactory
{
	CreateResult<IProject> Create(IApplication application);
}
