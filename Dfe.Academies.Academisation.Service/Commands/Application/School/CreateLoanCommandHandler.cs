using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository; 

	public CreateLoanCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(CreateLoanCommand loanCommand, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(loanCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();
			
		var result = existingApplication.CreateLoan(loanCommand.SchoolId, loanCommand.Amount, loanCommand.Purpose, loanCommand.Provider,
			loanCommand.InterestRate, loanCommand.Schedule);
			
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
