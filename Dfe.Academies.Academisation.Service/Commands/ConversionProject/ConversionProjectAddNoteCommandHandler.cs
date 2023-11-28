using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectAddNoteCommandHandler : IRequestHandler<ConversionProjectAddNoteCommand, CommandResult>
	{
		private readonly IProjectNoteAddCommand _addNoteCommand;
		private readonly IConversionProjectRepository _conversionProjectRepository;


		public ConversionProjectAddNoteCommandHandler(IConversionProjectRepository conversionProjectRepository,
										   IProjectNoteAddCommand addNoteCommand)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_addNoteCommand = addNoteCommand;
		}

		public async Task<CommandResult> Handle(ConversionProjectAddNoteCommand model, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(model.ProjectId);

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
