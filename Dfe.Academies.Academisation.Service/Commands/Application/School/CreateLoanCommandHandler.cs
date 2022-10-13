using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.CommandValidations;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class CreateLoanCommandHandler : ICreateLoanCommandHandler
{
	private readonly IApplicationRepository _applicationRepository; 
	private readonly IValidatorFactory<CreateLoanCommand> _validatorFactory;

	public CreateLoanCommandHandler(IApplicationRepository applicationRepository, IValidatorFactory<CreateLoanCommand> validatorFactory)
	{
		_applicationRepository = applicationRepository;
		_validatorFactory = validatorFactory;
	}

	public async Task<CommandResult> Handle(CreateLoanCommand loanCommand)
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
			
		var result = existingApplication.CreateLoan(loanCommand.SchoolId, loanCommand.Amount, loanCommand.Purpose, loanCommand.Provider,
			loanCommand.InterestRate, loanCommand.Schedule);
			
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
