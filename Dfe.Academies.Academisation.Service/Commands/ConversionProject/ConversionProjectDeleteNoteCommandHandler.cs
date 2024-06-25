using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectDeleteNoteCommandHandler : IRequestHandler<ConversionProjectDeleteNoteCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;

		public ConversionProjectDeleteNoteCommandHandler(IConversionProjectRepository conversionProjectRepository)
		{
			_conversionProjectRepository = conversionProjectRepository;
		}

		public async Task<CommandResult> Handle(ConversionProjectDeleteNoteCommand note, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(note.ProjectId, cancellationToken);

			if (project is null)
			{
				return new NotFoundCommandResult();
			}

			// we are doing this because the note id doesn't come from the client
			var matchedNote = project.Notes
					.FirstOrDefault(x => x.ProjectId == note.ProjectId &&
											  x.Subject == note.Subject &&
											  x.Note == note.Note &&
											  x.Author == note.Author &&
											  x.Date == note.Date);

			if (matchedNote is null)
			{
				return new NotFoundCommandResult();
			}

			project.RemoveNote(matchedNote.Id);

			_conversionProjectRepository.Update(project as Project);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
