using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class CreateLeaseCommandHandler : IRequestHandler<CreateLeaseCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository;
	public CreateLeaseCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(CreateLeaseCommand leaseCommand, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(leaseCommand.ApplicationId);
			
		if (existingApplication == null) return new NotFoundCommandResult();
			
		var result = existingApplication.CreateLease(leaseCommand.SchoolId, leaseCommand.LeaseTerm, leaseCommand.RepaymentAmount, leaseCommand.InterestRate, leaseCommand.PaymentsToDate, leaseCommand.Purpose, leaseCommand.ValueOfAssets, leaseCommand.ResponsibleForAssets);
			
		if (result is not CommandSuccessResult)
		{
			return result;
		}
			
		_applicationRepository.Update(existingApplication);
		return await _applicationRepository.UnitOfWork.SaveChangesAsync(new CancellationToken()) 
			? new CommandSuccessResult()
			: new BadRequestCommandResult();
	}
}
