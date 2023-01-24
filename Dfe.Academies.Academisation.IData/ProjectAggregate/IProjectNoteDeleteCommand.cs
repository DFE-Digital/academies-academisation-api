using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectNoteDeleteCommand
	{
		Task<CommandResult> Execute(int projectId, ProjectNote note);
	}
}
