using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class UpdateLeaseCommandHandler : IRequestHandler<UpdateLeaseCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository;
	public UpdateLeaseCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(UpdateLeaseCommand leaseCommand, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(leaseCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();

		var result = existingApplication.UpdateLease(leaseCommand.SchoolId, leaseCommand.LeaseId, leaseCommand.LeaseTerm, leaseCommand.RepaymentAmount, leaseCommand.InterestRate, leaseCommand.PaymentsToDate, leaseCommand.Purpose, leaseCommand.ValueOfAssets, leaseCommand.ResponsibleForAssets);

		if (result is not CommandSuccessResult)
		{
			return result;
		}

		_applicationRepository.Update(existingApplication);
		return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(new CancellationToken())
			? new CommandSuccessResult()
			: new BadRequestCommandResult();
	}
}
