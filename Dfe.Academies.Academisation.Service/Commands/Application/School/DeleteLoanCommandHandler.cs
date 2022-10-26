using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class DeleteLoanCommandHandler : IDeleteLoanCommandHandler
{
	private readonly IApplicationRepository _applicationRepository; 

	public DeleteLoanCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(DeleteLoanCommand loanCommand)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(loanCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();

		var result = existingApplication.DeleteLoan(loanCommand.SchoolId, loanCommand.LoanId);
		if (result is not CommandSuccessResult)
		{
			return result;
		}

		_applicationRepository.Update(existingApplication);
		
		//TODO: This can be removed when there is no longer a disconnect between domain and persistence entities
		await _applicationRepository.DeleteChildObjectById<LoanState>(loanCommand.LoanId);
		
		return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(new CancellationToken()) 
			? new CommandSuccessResult()
			: new BadRequestCommandResult();
	}
}
