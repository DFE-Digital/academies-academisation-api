using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public class SetSignificantChangeProjectDatesCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeProjectDatesCommandHandler> logger): IRequestHandler<SetSignificantChangeProjectDatesCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetSignificantChangeProjectDatesCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

			if (existingProject is null)
			{
				logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
				return new NotFoundCommandResult();
			}

			existingProject.SetProjectDates(request.ProposedDecisionDate, request.ProposedChangeDate);

			repository.Update(existingProject);
			await repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}
