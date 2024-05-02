using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectAddNoteCommandHandler : IRequestHandler<ConversionProjectAddNoteCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;


		public ConversionProjectAddNoteCommandHandler(IConversionProjectRepository conversionProjectRepository)
		{
			_conversionProjectRepository = conversionProjectRepository;
		}

		public async Task<CommandResult> Handle(ConversionProjectAddNoteCommand model, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(model.ProjectId);

			if (project is null)
			{
				return new NotFoundCommandResult();
			}

			project.AddNote(model.Subject,
					model.Note,
					model.Author,
					model.Date);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync();

			return new CommandSuccessResult();

		}
	}
}
