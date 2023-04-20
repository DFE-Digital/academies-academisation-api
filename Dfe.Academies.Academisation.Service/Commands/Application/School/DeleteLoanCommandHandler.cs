using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository; 

	public DeleteLoanCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(DeleteLoanCommand loanCommand, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(loanCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();

		var result = existingApplication.DeleteLoan(loanCommand.SchoolId, loanCommand.LoanId);
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
