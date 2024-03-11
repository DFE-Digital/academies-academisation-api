using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.FormAMat
{
	public class SetFormAMatReferenceNumberCommandHandler : IRequestHandler<SetFormAMatReferenceNumberCommand, CommandResult>
	{
		private readonly IFormAMatProjectRepository _formAMatProjectRepository;
		private readonly ILogger<SetFormAMatReferenceNumberCommandHandler> _logger;

		public SetFormAMatReferenceNumberCommandHandler(
			IFormAMatProjectRepository formAMatProjectRepository,
			ILogger<SetFormAMatReferenceNumberCommandHandler> logger)
		{
			_formAMatProjectRepository = formAMatProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetFormAMatReferenceNumberCommand request, CancellationToken cancellationToken)
		{
			var formAMatProjectsWithoutReferenceNumbers = await _formAMatProjectRepository.GetProjectsWithoutReference(cancellationToken).ConfigureAwait(false);

			if (formAMatProjectsWithoutReferenceNumbers == null || !formAMatProjectsWithoutReferenceNumbers.Any())
			{
				_logger.LogInformation($"No Form a MAT projects found without a reference");
				return new NotFoundCommandResult();
			}

			foreach (IDomain.FormAMatProjectAggregate.IFormAMatProject famProject in formAMatProjectsWithoutReferenceNumbers)
			{
				famProject.SetProjectReference(famProject.Id);

				_formAMatProjectRepository.Update(famProject as FormAMatProject);
			}
			await _formAMatProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
