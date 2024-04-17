using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionUpdateCommand : IAdvisoryBoardDecisionUpdateCommand
{
	private readonly IAdvisoryBoardDecisionGetDataByDecisionIdQuery _getDataQuery;
	private readonly IAdvisoryBoardDecisionUpdateDataCommand _updateDataCommand;

	public AdvisoryBoardDecisionUpdateCommand(
		IAdvisoryBoardDecisionUpdateDataCommand updateDataCommand,
		IAdvisoryBoardDecisionGetDataByDecisionIdQuery getDataQuery)
	{
		_updateDataCommand = updateDataCommand;
		_getDataQuery = getDataQuery;
	}

	public async Task<CommandResult> Execute(ConversionAdvisoryBoardDecisionServiceModel serviceModel)
	{
		if (serviceModel.AdvisoryBoardDecisionId == default)
		{
			return new BadRequestCommandResult();
		}

		var existingDecision = await _getDataQuery.Execute(serviceModel.AdvisoryBoardDecisionId);

		if (existingDecision is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingDecision.Update(serviceModel.ToDomain());

		return result switch
		{
			CommandSuccessResult => await ExecuteDataCommand(existingDecision),
			CommandValidationErrorResult errorResult => errorResult,
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CommandResult> ExecuteDataCommand(IConversionAdvisoryBoardDecision decision)
	{
		await _updateDataCommand.Execute(decision);

		return new CommandSuccessResult();
	}
}
