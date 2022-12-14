using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Extensions;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommand : ILegacyProjectAddNoteCommand
	{
		private readonly IProjectGetDataQuery _projectGetDataQuery;
		private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

		public LegacyProjectAddNoteCommand(IProjectGetDataQuery projectGetDataQuery, IProjectUpdateDataCommand projectUpdateDataCommand)
		{
			_projectGetDataQuery = projectGetDataQuery;
			_projectUpdateDataCommand = projectUpdateDataCommand;
		}

		public async Task<CommandResult> Execute(LegacyProjectAddNoteModel model)
		{
			IProject? project = await _projectGetDataQuery.Execute(model.ProjectId);

			if (project is null) return new NotFoundCommandResult();

			project.Details.Notes.Add(model.ToProjectNote());
			await _projectUpdateDataCommand.Execute(project);

			return new CommandSuccessResult();
		}
	}
}
