using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

internal class AdvisoryBoardDecisionCreateCommandHandler(
	IConversionAdvisoryBoardDecisionFactory factory,
	IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
	IConversionProjectRepository conversionProjectRepository,
	ITransferProjectRepository transferProjectRepository,
	ISignificantChangeProjectRepository significantChangeProjectRepository,
	IDateTimeProvider dateTimeProvider)
	: AdvisoryBoardDecisionProjectReadOnlyUpdater(transferProjectRepository, conversionProjectRepository, significantChangeProjectRepository, dateTimeProvider),
	  IRequestHandler<AdvisoryBoardDecisionCreateCommand, CreateResult>,
	  IRequestHandler<SignificantChangeDecisionCommand, CreateResult>
{
	public async Task<CreateResult> Handle(AdvisoryBoardDecisionCreateCommand request,
		CancellationToken cancellationToken)
	{
		var result = CreateAdvisoryBoardDecisionDetails(request);

		return result switch
		{
			CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult =>
				await ExecuteDataCommand<ConversionAdvisoryBoardDecisionServiceModel>(successResult, cancellationToken),
			CreateValidationErrorResult errorResult =>
				errorResult.MapToPayloadType(),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	public async Task<CreateResult> Handle(SignificantChangeDecisionCommand request, CancellationToken cancellationToken)
	{
		var result = CreateSignificantChangeDecisionDetails(request);

		return result switch
		{
			CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult =>
				await ExecuteDataCommand<SignificantChangeDecisionServiceModel>(successResult, cancellationToken),
			CreateValidationErrorResult errorResult =>
				errorResult.MapToPayloadType(),
			_ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
		};
	}

	private async Task<CreateResult> ExecuteDataCommand<TServiceModel>(
		CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult, CancellationToken cancellationToken)
	{
		advisoryBoardDecisionRepository.Insert(successResult.Payload as ConversionAdvisoryBoardDecision);

		await advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		if (successResult.Payload.AdvisoryBoardDecisionDetails.Decision == Domain.Core
			    .ConversionAdvisoryBoardDecisionAggregate
			    .AdvisoryBoardDecision.Approved)
		{
			await SetProjectReadOnlyAsync(successResult.Payload.AdvisoryBoardDecisionDetails);
		}

		return successResult.MapToPayloadType(GetMapFromDomain<TServiceModel>());
	}

	private static Func<IConversionAdvisoryBoardDecision, TServiceModel> GetMapFromDomain<TServiceModel>()
	{
		if (typeof(TServiceModel) == typeof(ConversionAdvisoryBoardDecisionServiceModel))
		{
			return decision => (TServiceModel)(object)ConversionAdvisoryBoardDecisionServiceModelMapper.MapFromDomain(decision);
		}

		if (typeof(TServiceModel) == typeof(SignificantChangeDecisionServiceModel))
		{
			return decision => (TServiceModel)(object)SignificantChangeDecisionServiceModelMapper.MapFromDomain(decision);
		}

		throw new NotSupportedException($"No MapFromDomain mapper configured for service model type '{typeof(TServiceModel).Name}'.");
	}

	private CreateResult CreateAdvisoryBoardDecisionDetails(AdvisoryBoardDecisionCreateCommand request)
	{
		var details = new AdvisoryBoardDecisionDetails(
			request.ConversionProjectId,
			request.TransferProjectId,
			null,
			request.Decision,
			request.ApprovedConditionsSet,
			request.ApprovedConditionsDetails,
			request.AdvisoryBoardDecisionDate,
			request.AcademyOrderDate,
			request.DecisionMadeBy,
			request.DecisionMakerName
		);
		
		var deferredReasons = request.DeferredReasons ?? [];
		var declinedReasons = request.DeclinedReasons ?? [];
		var withdrawnReasons = request.WithdrawnReasons ?? [];
		var daoRevokedReasons = request.DAORevokedReasons ?? [];

		return factory.Create(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons);
	}

	private CreateResult CreateSignificantChangeDecisionDetails(SignificantChangeDecisionCommand request)
	{
		var significantChangeDetails = new AdvisoryBoardDecisionDetails(
			null,
			null,
			request.SignificantChangeProjectId,
			request.Decision.MapToAdvisoryBoardDecision(),
			request.ApprovedConditionsSet,
			request.ApprovedConditionsDetails,
			request.DecisionDate,
			null,
			request.DecisionMadeBy,
			request.DecisionMakerName
		);
		var sigChangeDeferredReasons = request.DeferredReasons ?? [];
		var sigChangeDeclinedReasons = request.DeclinedReasons ?? [];
		var sigChangeWithdrawnReasons = request.WithdrawnReasons ?? [];

		return factory.Create(significantChangeDetails, sigChangeDeferredReasons, sigChangeDeclinedReasons, sigChangeWithdrawnReasons, []);
	}
	
}
