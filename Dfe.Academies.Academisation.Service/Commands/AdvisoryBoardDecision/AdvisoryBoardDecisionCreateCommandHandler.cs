using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionCreateCommandHandler(
	IConversionAdvisoryBoardDecisionFactory factory,
	IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
	IConversionProjectRepository conversionProjectRepository,
	ITransferProjectRepository transferProjectRepository,
	IDateTimeProvider dateTimeProvider) : IRequestHandler<AdvisoryBoardDecisionCreateCommand, CreateResult>
{
	public async Task<CreateResult> Handle(AdvisoryBoardDecisionCreateCommand request, CancellationToken cancellationToken)
	{
		var deferredReasons = request.DeferredReasons ?? [];
		var declinedReasons = request.DeclinedReasons ?? [];
		var withdrawnReasons = request.WithdrawnReasons ?? [];
		var daoRevokedReasons = request.DAORevokedReasons ?? [];

		var details = new AdvisoryBoardDecisionDetails(
			request.ConversionProjectId,
			request.TransferProjectId,
			request.Decision,
			request.ApprovedConditionsSet,
			request.ApprovedConditionsDetails,
			request.AdvisoryBoardDecisionDate,
			request.AcademyOrderDate,
			request.DecisionMadeBy,
			request.DecisionMakerName
		);

		var result = factory.Create(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons);

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
		advisoryBoardDecisionRepository.Insert(successResult.Payload as ConversionAdvisoryBoardDecision);

		await advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
		await SetProjectReadOnlyAsync(successResult.Payload.AdvisoryBoardDecisionDetails);
		return successResult.MapToPayloadType(ConversionAdvisoryBoardDecisionServiceModelMapper.MapFromDomain);
	}

	private async Task SetProjectReadOnlyAsync(AdvisoryBoardDecisionDetails advisoryBoardDecisionDetails)
	{
		if (advisoryBoardDecisionDetails != null)
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
}
