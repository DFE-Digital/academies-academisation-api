using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

internal class AdvisoryBoardDecisionProjectReadOnlyUpdater(
	ITransferProjectRepository transferProjectRepository,
	IConversionProjectRepository conversionProjectRepository,
	ISignificantChangeProjectRepository significantChangeProjectRepository,
	IDateTimeProvider dateTimeProvider)
{
	protected async Task SetProjectReadOnlyAsync(AdvisoryBoardDecisionDetails advisoryBoardDecisionDetails)
	{
		if (advisoryBoardDecisionDetails.TransferProjectId != null)
		{
			await UpdateTransferProjectAsync(advisoryBoardDecisionDetails.TransferProjectId);
		}
		else if (advisoryBoardDecisionDetails.ConversionProjectId != null)
		{
			await UpdateConversionProjectAsync(advisoryBoardDecisionDetails.ConversionProjectId);
		}
		else if (advisoryBoardDecisionDetails.SignificantChangeProjectId != null)
		{
			await UpdateSignificantChangeProjectAsync(advisoryBoardDecisionDetails.SignificantChangeProjectId);
		}
	}

	private async Task UpdateSignificantChangeProjectAsync(int? significantChangeProjectId)
	{
		var project = await significantChangeProjectRepository.GetById(significantChangeProjectId.GetValueOrDefault());

		if (project != null)
		{
			project.SetReadOnlyDate(dateTimeProvider.Now);

			significantChangeProjectRepository.Update(project);
			await significantChangeProjectRepository.UnitOfWork.SaveChangesAsync();
		}
	}

	private async Task UpdateConversionProjectAsync(int? conversionProjectId)
	{
		var project = await conversionProjectRepository.GetById(conversionProjectId.GetValueOrDefault());

		if (project != null)
		{
			project.SetIsReadOnly(dateTimeProvider.Now);

			conversionProjectRepository.Update(project);
			await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
		}
	}

	private async Task UpdateTransferProjectAsync(int? transferProjectId)
	{
		var project = await transferProjectRepository.GetById(transferProjectId.GetValueOrDefault());

		if (project != null)
		{
			project.SetIsReadOnly(dateTimeProvider.Now);

			transferProjectRepository.Update(project);
			await transferProjectRepository.UnitOfWork.SaveChangesAsync();
		}
	}
}
