using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionCreateCommandHandler : IRequestHandler<AdvisoryBoardDecisionCreateCommand, CreateResult>
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
	private readonly IConversionAdvisoryBoardDecisionFactory _factory;

	public AdvisoryBoardDecisionCreateCommandHandler(IConversionAdvisoryBoardDecisionFactory factory,
		IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
		_factory = factory;
	}

	public async Task<CreateResult> Handle(AdvisoryBoardDecisionCreateCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons = request.DeferredReasons ?? new List<AdvisoryBoardDeferredReasonDetails>();
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons = request.DeclinedReasons ?? new List<AdvisoryBoardDeclinedReasonDetails>();
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons = request.WithdrawnReasons ?? new List<AdvisoryBoardWithdrawnReasonDetails>();

		var details = new AdvisoryBoardDecisionDetails(
			request.ConversionProjectId,
			request.TransferProjectId,
			request.Decision,
			request.ApprovedConditionsSet,
			request.ApprovedConditionsDetails,
			request.AdvisoryBoardDecisionDate,
			request.AcademyOrderDate,
			request.DecisionMadeBy,
			request.DecisionMakersName
		);

		var result = _factory.Create(details, deferredReasons, declinedReasons, withdrawnReasons);

		return result switch
		{
			CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult =>
				await ExecuteDataCommand(successResult, cancellationToken),
			CreateValidationErrorResult errorResult =>
				errorResult.MapToPayloadType(),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CreateResult> ExecuteDataCommand(
		CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult, CancellationToken cancellationToken)
	{
		_advisoryBoardDecisionRepository.Insert(successResult.Payload as ConversionAdvisoryBoardDecision);

		await _advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return successResult.MapToPayloadType(ConversionAdvisoryBoardDecisionServiceModelMapper.MapFromDomain);
	}
}
