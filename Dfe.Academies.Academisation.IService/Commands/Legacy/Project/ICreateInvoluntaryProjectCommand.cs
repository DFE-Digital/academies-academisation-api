using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface ICreateInvoluntaryProjectCommand
	{
		Task<CommandOrCreateResult> Execute(InvoluntaryProject project);
	}
}
