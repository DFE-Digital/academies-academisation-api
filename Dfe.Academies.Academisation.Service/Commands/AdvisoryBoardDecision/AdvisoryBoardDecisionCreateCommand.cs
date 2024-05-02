using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionCreateCommand : IAdvisoryBoardDecisionCreateCommand
{
	private readonly IAdvisoryBoardDecisionCreateDataCommand _createDataCommand;
	private readonly IConversionAdvisoryBoardDecisionFactory _factory;

	public AdvisoryBoardDecisionCreateCommand(IConversionAdvisoryBoardDecisionFactory factory,
		IAdvisoryBoardDecisionCreateDataCommand createDataCommand)
	{
		_createDataCommand = createDataCommand;
		_factory = factory;
	}

	public async Task<CreateResult> Execute(
		AdvisoryBoardDecisionCreateRequestModel requestModel)
	{
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons = requestModel.DeferredReasons ?? new List<AdvisoryBoardDeferredReasonDetails>();
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons = requestModel.DeclinedReasons ?? new List<AdvisoryBoardDeclinedReasonDetails>();
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons = requestModel.WithdrawnReasons ?? new List<AdvisoryBoardWithdrawnReasonDetails>();

		var result = _factory.Create(requestModel.AsDomain(), deferredReasons, declinedReasons, withdrawnReasons);

		return result switch
		{
			CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult =>
				await ExecuteDataCommand(successResult),
			CreateValidationErrorResult errorResult =>
				errorResult.MapToPayloadType(),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CreateResult> ExecuteDataCommand(
		CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult)
	{
		await _createDataCommand.Execute(successResult.Payload);

		return successResult.MapToPayloadType(ConversionAdvisoryBoardDecisionServiceModelMapper.MapFromDomain);
	}
}
