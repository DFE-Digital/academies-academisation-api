using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School
{
	public class DeleteLeaseCommandHandler : IDeleteLeaseCommandHandler
	{
		private readonly IApplicationRepository _applicationRepository; 

		public DeleteLeaseCommandHandler(IApplicationRepository applicationRepository)
		{
			_applicationRepository = applicationRepository;
		}

		public async Task<CommandResult> Handle(DeleteLeaseCommand leaseCommand)
		{
			var existingApplication = await _applicationRepository.GetByIdAsync(leaseCommand.applicationId);
			if (existingApplication == null) return new NotFoundCommandResult();

			var result = existingApplication.DeleteLease(leaseCommand.schoolId, leaseCommand.leaseId);
			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}
			
			_applicationRepository.Update(existingApplication);
			return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(new CancellationToken()) 
				? new CommandSuccessResult()
				: new BadRequestCommandResult();
		}
	}
}
