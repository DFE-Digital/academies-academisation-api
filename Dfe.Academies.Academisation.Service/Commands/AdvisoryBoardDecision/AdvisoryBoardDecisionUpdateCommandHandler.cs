using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionUpdateCommandHandler : IRequestHandler<AdvisoryBoardDecisionUpdateCommand, CommandResult>
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

	public AdvisoryBoardDecisionUpdateCommandHandler(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
	}


	public async Task<CommandResult> Handle(AdvisoryBoardDecisionUpdateCommand command, CancellationToken cancellationToken)
	{
		if (command.AdvisoryBoardDecisionId == default)
		{
			return new BadRequestCommandResult();
		}

		var existingDecision = await _advisoryBoardDecisionRepository.GetAdvisoryBoardDecisionById(command.AdvisoryBoardDecisionId);

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

		var result = existingDecision.Update(details, command.DeferredReasons, command.DeclinedReasons, command.WithdrawnReasons);

		return result switch
		{
			CommandSuccessResult => await ExecuteDataCommand(existingDecision, cancellationToken),
			CommandValidationErrorResult errorResult => errorResult,
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CommandResult> ExecuteDataCommand(IConversionAdvisoryBoardDecision decision, CancellationToken cancellationToken)
	{
		_advisoryBoardDecisionRepository.Update(decision as ConversionAdvisoryBoardDecision);


		await _advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return new CommandSuccessResult();
	}
}
