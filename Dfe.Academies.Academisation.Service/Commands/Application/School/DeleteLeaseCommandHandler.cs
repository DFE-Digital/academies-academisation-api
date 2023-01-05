using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class DeleteLeaseCommandHandler : IRequestHandler<DeleteLeaseCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository; 

	public DeleteLeaseCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(DeleteLeaseCommand leaseCommand, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(leaseCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();

		var result = existingApplication.DeleteLease(leaseCommand.SchoolId, leaseCommand.LeaseId);
		if (result is not CommandSuccessResult)
		{
			return result;
		}
		
		_applicationRepository.Update(existingApplication);

		return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken) 
			? new CommandSuccessResult()
			: new BadRequestCommandResult();
	}
}
