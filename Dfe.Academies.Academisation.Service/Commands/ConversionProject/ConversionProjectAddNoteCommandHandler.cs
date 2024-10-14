using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectAddNoteCommandHandler(IConversionProjectRepository conversionProjectRepository) : IRequestHandler<ConversionProjectAddNoteCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(ConversionProjectAddNoteCommand model, CancellationToken cancellationToken)
		{
			IProject? project = await conversionProjectRepository.GetConversionProject(model.ProjectId, cancellationToken);

			if (project is null)
			{
				return new NotFoundCommandResult();
			}

			project.AddNote(model.Subject,
					model.Note,
					model.Author,
					model.Date);

			await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();

		}
	}
}
