using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class SetTransferProjectBenefitsCommandHandler : IRequestHandler<SetTransferProjectBenefitsCommand, CommandResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	private readonly ILogger<SetTransferProjectBenefitsCommandHandler> _logger;

	public SetTransferProjectBenefitsCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectBenefitsCommandHandler> logger)
	{
		_transferProjectRepository = transferProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetTransferProjectBenefitsCommand request, CancellationToken cancellationToken)
	{
		var transferProject = await _transferProjectRepository.GetById(request.Id).ConfigureAwait(false);

		if (transferProject == null) {
			_logger.LogError($"transfer project not found with id:{request.Id}");
			return new NotFoundCommandResult();
		}
		transferProject.SetBenefitsAndRisks(
			request.AnyRisks, 
			request.EqualitiesImpactAssessmentConsidered,
			
			request.IntendedTransferBenefits.SelectedBenefits, 
			request.IntendedTransferBenefits.OtherBenefitValue,

			request.OtherFactorsToConsider.HighProfile.ShouldBeConsidered,
			request.OtherFactorsToConsider.HighProfile.FurtherSpecification,
			request.OtherFactorsToConsider.ComplexLandAndBuilding.ShouldBeConsidered,
			request.OtherFactorsToConsider.ComplexLandAndBuilding.FurtherSpecification,
			request.OtherFactorsToConsider.FinanceAndDebt.ShouldBeConsidered,
			request.OtherFactorsToConsider.FinanceAndDebt.FurtherSpecification,
			request.OtherFactorsToConsider.OtherRisks.ShouldBeConsidered,
			request.OtherFactorsToConsider.OtherRisks.FurtherSpecification,

			request.IsCompleted);

		 _transferProjectRepository.Update(transferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
		return new CommandSuccessResult();
	}
}
