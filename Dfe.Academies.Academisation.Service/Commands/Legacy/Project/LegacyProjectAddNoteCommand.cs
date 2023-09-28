using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommand : ILegacyProjectAddNoteCommand
	{
		private readonly IProjectNoteAddCommand _addNoteCommand;
		private readonly IProjectGetDataQuery _projectGetDataQuery;


		public LegacyProjectAddNoteCommand(IProjectGetDataQuery projectGetDataQuery,
										   IProjectNoteAddCommand addNoteCommand)
		{
			_projectGetDataQuery = projectGetDataQuery;
			_addNoteCommand = addNoteCommand;
		}

		public async Task<CommandResult> Execute(LegacyProjectAddNoteModel model)
		{
			IProject? project = await _projectGetDataQuery.Execute(model.ProjectId);

			if (project is null)
			{
				return new NotFoundCommandResult();
			}

			return await _addNoteCommand.Execute(
				model.ProjectId,
				new ProjectNote(
					model.Subject,
					model.Note,
					model.Author,
					model.Date, model.ProjectId)
			);
		}
	}
}
