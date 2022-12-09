using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommand : ILegacyProjectAddNoteCommand
	{
		public Task<CommandResult> Execute(LegacyProjectAddNoteModel model)
		{
			throw new NotImplementedException("LegacyProjectAddNoteCommand.Execute");
		}
	}
}
