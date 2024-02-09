using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.Service.Commands.FormAMat
{

	public class SetFormAMatAssignedUserCommandHandler : IRequestHandler<SetFormAMatAssignedUserCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IFormAMatProjectRepository _formAMatProjectRepository;
		private readonly ILogger<SetFormAMatAssignedUserCommandHandler> _logger;

		public SetFormAMatAssignedUserCommandHandler(
			IConversionProjectRepository conversionProjectRepository,
			IFormAMatProjectRepository formAMatProjectRepository,
			ILogger<SetFormAMatAssignedUserCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_formAMatProjectRepository = formAMatProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetFormAMatAssignedUserCommand request, CancellationToken cancellationToken)
		{
			var firstProject = await _conversionProjectRepository.GetConversionProject(request.Id);
			// Set user on parent Form a MAT object
			var formAMatProject = await _formAMatProjectRepository.GetById((int)firstProject.FormAMatProjectId);
			if (formAMatProject is null)
			{
				_logger.LogError($"Form a MAT project not found from conversion project with id: {request.Id}");
				return new NotFoundCommandResult();
			}
			formAMatProject.SetAssignedUser(request.UserId, request.FullName, request.EmailAddress);

			_formAMatProjectRepository.Update(formAMatProject);

			// Get All Projects in this FAM by FAM Id
			var otherProjectsInFormAMat = await _conversionProjectRepository.GetConversionProjectsByFormAMatId(firstProject.FormAMatProjectId, cancellationToken);
			//foreach project assign the user unless one is already assigned
			foreach (var project in otherProjectsInFormAMat)
			{
				if (project is null)
				{
					_logger.LogError($"Conversion project not found with id: {request.Id}");
					return new NotFoundCommandResult();
				}
				if (project.Details.AssignedUser.FullName.IsNullOrEmpty())
				{
					project.SetAssignedUser(request.UserId, request.FullName, request.EmailAddress);

					_conversionProjectRepository.Update(project as Project);
				}
			}

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}
