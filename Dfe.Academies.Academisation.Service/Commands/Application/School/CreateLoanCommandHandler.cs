using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School
{
	public class CreateLoanCommandHandler : ICreateLoanCommandHandler
	{
		private readonly IApplicationRepository _applicationRepository; 

		public CreateLoanCommandHandler(IApplicationRepository applicationRepository)
		{
			_applicationRepository = applicationRepository;
		}

		public async Task<CommandResult> Handle(CreateLoanCommand loanCommand)
		{
			var existingApplication = await _applicationRepository.GetByIdAsync(loanCommand.applicationId);
			if (existingApplication == null) return new NotFoundCommandResult();
			
			var result = existingApplication.CreateLoan(loanCommand.schoolId, loanCommand.amount, loanCommand.purpose, loanCommand.provider,
				loanCommand.interestRate, loanCommand.schedule);
			
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
