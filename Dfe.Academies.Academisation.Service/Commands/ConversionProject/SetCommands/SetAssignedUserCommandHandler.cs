using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetAssignedUserCommandHandler : IRequestHandler<SetAssignedUserCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly ILogger<SetAssignedUserCommandHandler> _logger;

		public SetAssignedUserCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<SetAssignedUserCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetAssignedUserCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await _conversionProjectRepository.GetConversionProject(request.Id, cancellationToken);

			if (existingProject is null)
			{
				_logger.LogError($"Conversion project not found with id: {request.Id}");
				return new NotFoundCommandResult();
			}

			// Update the school overview information in the existing project
			existingProject.SetAssignedUser(request.UserId, request.FullName, request.EmailAddress);

			_conversionProjectRepository.Update(existingProject as Project);
			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}

