using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateProjectGroupCommand, CommandResult>
	{

		public async Task<CommandResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(
			message.TrustReference,
			message.ReferenceNumber,
			dateTimeProvider.Now);

			projectGroupRepository.Insert(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
