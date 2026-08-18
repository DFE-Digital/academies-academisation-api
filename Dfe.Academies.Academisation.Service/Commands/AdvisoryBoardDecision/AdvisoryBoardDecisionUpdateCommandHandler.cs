using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

internal class AdvisoryBoardDecisionUpdateCommandHandler(
	IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository, 
	IConversionProjectRepository conversionProjectRepository,
	ITransferProjectRepository transferProjectRepository,
	ISignificantChangeProjectRepository significantChangeProjectRepository,
	IDateTimeProvider dateTimeProvider)
	: AdvisoryBoardDecisionProjectReadOnlyUpdater(transferProjectRepository, conversionProjectRepository, significantChangeProjectRepository, dateTimeProvider),
	  IRequestHandler<AdvisoryBoardDecisionUpdateCommand, CommandResult>,
	  IRequestHandler<SignificantChangeUpdateDecisionCommand, CommandResult>
{
	public async Task<CommandResult> Handle(AdvisoryBoardDecisionUpdateCommand command, CancellationToken cancellationToken)
	{
		var details = new AdvisoryBoardDecisionDetails(
			command.ConversionProjectId,
			command.TransferProjectId,
			command.SignificantChangeProjectId,
			command.Decision,
			command.ApprovedConditionsSet,
			command.ApprovedConditionsDetails,
			command.AdvisoryBoardDecisionDate,
			command.AcademyOrderDate,
			command.DecisionMadeBy,
			command.DecisionMakerName
		);

		return await UpdateDecisionAsync(
			command.AdvisoryBoardDecisionId,
			existingDecision => existingDecision.Update(details, command.DeferredReasons!, command.DeclinedReasons!, command.WithdrawnReasons!, command.DAORevokedReasons!),
			cancellationToken);
	}

	public async Task<CommandResult> Handle(SignificantChangeUpdateDecisionCommand command, CancellationToken cancellationToken)
	{
		var details = new AdvisoryBoardDecisionDetails(
			null,
			null,
			command.SignificantChangeProjectId,
			command.Decision.MapToAdvisoryBoardDecision(),
			command.ApprovedConditionsSet,
			command.ApprovedConditionsDetails,
			command.DecisionDate,
			null,
			command.DecisionMadeBy,
			command.DecisionMakerName
		);

		return await UpdateDecisionAsync(
			command.AdvisoryBoardDecisionId,
			existingDecision => existingDecision.Update(details, command.DeferredReasons!, command.DeclinedReasons!, command.WithdrawnReasons!, new List<AdvisoryBoardDAORevokedReasonDetails>()),
			cancellationToken);
	}

	private async Task<CommandResult> UpdateDecisionAsync(
		int advisoryBoardDecisionId,
		Func<IConversionAdvisoryBoardDecision, CommandResult> updateDecision,
		CancellationToken cancellationToken)
	{
		if (advisoryBoardDecisionId == default)
		{
			return new BadRequestCommandResult();
		}

		var existingDecision = await advisoryBoardDecisionRepository.GetAdvisoryBoardDecisionById(advisoryBoardDecisionId);

		if (existingDecision is null)
		{
			return new NotFoundCommandResult();
		}

		var result = updateDecision(existingDecision);

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

		if (decision.AdvisoryBoardDecisionDetails.Decision == Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved)
		{
			await SetProjectReadOnlyAsync(decision.AdvisoryBoardDecisionDetails);
		}

		return new CommandSuccessResult();
	}

}
