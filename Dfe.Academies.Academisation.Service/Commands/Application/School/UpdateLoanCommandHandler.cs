using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.CommandValidations;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class UpdateLoanCommandHandler : IUpdateLoanCommandHandler
{
	private readonly IApplicationRepository _applicationRepository;
	private readonly IValidatorFactory<UpdateLoanCommand> _validatorFactory;
	public UpdateLoanCommandHandler(IApplicationRepository applicationRepository, IValidatorFactory<UpdateLoanCommand> validatorFactory)
	{
		_applicationRepository = applicationRepository;
		_validatorFactory = validatorFactory;
	}

	public async Task<CommandResult> Handle(UpdateLoanCommand loanCommand)
	{
		var validator = _validatorFactory.GetCommandValidator();
		var validationResult = await validator.ValidateAsync(loanCommand);

		if (!validationResult.IsValid || validationResult.Errors.Any())
		{
			var validationErrors = new List<ValidationError>();
			validationErrors.AddRange(validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
			return new CommandValidationErrorResult(validationErrors);
		}
		
		var existingApplication = await _applicationRepository.GetByIdAsync(loanCommand.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();
			
		var result = existingApplication.UpdateLoan(loanCommand.SchoolId, loanCommand.LoanId, loanCommand.Amount, loanCommand.Purpose, loanCommand.Provider,
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
