using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface ILegacyProjectAddNoteCommand
	{
		Task<CommandResult> Execute(LegacyProjectAddNoteModel model);
	}
}
