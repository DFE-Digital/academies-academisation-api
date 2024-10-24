using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionUpdateCommandHandler(
	IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository, 
	IConversionProjectRepository conversionProjectRepository,
	ITransferProjectRepository transferProjectRepository,
	IDateTimeProvider dateTimeProvider) : IRequestHandler<AdvisoryBoardDecisionUpdateCommand, CommandResult>
{
	public async Task<CommandResult> Handle(AdvisoryBoardDecisionUpdateCommand command, CancellationToken cancellationToken)
	{
		if (command.AdvisoryBoardDecisionId == default)
		{
			return new BadRequestCommandResult();
		}

		var existingDecision = await advisoryBoardDecisionRepository.GetAdvisoryBoardDecisionById(command.AdvisoryBoardDecisionId);

		if (existingDecision is null)
		{
			return new NotFoundCommandResult();
		}

		var details = new AdvisoryBoardDecisionDetails(
			command.ConversionProjectId,
			command.TransferProjectId,
			command.Decision,
			command.ApprovedConditionsSet,
			command.ApprovedConditionsDetails,
			command.AdvisoryBoardDecisionDate,
			command.AcademyOrderDate,
			command.DecisionMadeBy,
			command.DecisionMakerName
		);

		var result = existingDecision.Update(details, command.DeferredReasons!, command.DeclinedReasons!, command.WithdrawnReasons!, command.DAORevokedReasons!);

		return result switch
		{
			CommandSuccessResult => await ExecuteDataCommand(existingDecision, cancellationToken),
			CommandValidationErrorResult errorResult => errorResult,
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CommandResult> ExecuteDataCommand(IConversionAdvisoryBoardDecision decision, CancellationToken cancellationToken)
	{
		advisoryBoardDecisionRepository.Update(decision as ConversionAdvisoryBoardDecision);

		await advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		await SetProjectReadOnlyAsync(decision.AdvisoryBoardDecisionDetails);

		return new CommandSuccessResult();
	}
	private async Task SetProjectReadOnlyAsync(AdvisoryBoardDecisionDetails advisoryBoardDecisionDetails)
	{
		if (advisoryBoardDecisionDetails.TransferProjectId != null)
		{
			var project = await transferProjectRepository.GetById(advisoryBoardDecisionDetails.TransferProjectId.GetValueOrDefault());
			if (project != null)
			{
				if (advisoryBoardDecisionDetails.Decision == Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved)
				{
					project.SetIsReadOnly(dateTimeProvider.Now);
				}
				transferProjectRepository.Update(project);
				await transferProjectRepository.UnitOfWork.SaveChangesAsync();
			}
		}
		else
		{
			var project = await conversionProjectRepository.GetById(advisoryBoardDecisionDetails.ConversionProjectId.GetValueOrDefault());
			if (project != null)
			{
				if (advisoryBoardDecisionDetails.Decision == Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved)
				{
					project.SetIsReadOnly(dateTimeProvider.Now);
				}
				conversionProjectRepository.Update(project);
				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}
		}
	}
}
