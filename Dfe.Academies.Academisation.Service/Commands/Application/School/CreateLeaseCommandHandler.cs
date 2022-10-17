using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.CommandValidations;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class CreateLeaseCommandHandler : ICreateLeaseCommandHandler
{
	private readonly IApplicationRepository _applicationRepository;
	private readonly IValidatorFactory<CreateLeaseCommand> _validatorFactory;
	public CreateLeaseCommandHandler(IApplicationRepository applicationRepository, IValidatorFactory<CreateLeaseCommand> validatorFactory)
	{
		_applicationRepository = applicationRepository;
		_validatorFactory = validatorFactory;
	}

	public async Task<CommandResult> Handle(CreateLeaseCommand leaseCommand)
	{
		var validator = _validatorFactory.GetCommandValidator();
		var validationResult = await validator.ValidateAsync(leaseCommand);

		if (!validationResult.IsValid || validationResult.Errors.Any())
		{
			var validationErrors = new List<ValidationError>();
			validationErrors.AddRange(validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
			return new CommandValidationErrorResult(validationErrors);
		}
			
		var existingApplication = await _applicationRepository.GetByIdAsync(leaseCommand.ApplicationId);
			
		if (existingApplication == null) return new NotFoundCommandResult();
			
		var result = existingApplication.CreateLease(leaseCommand.SchoolId, leaseCommand.LeaseTerm, leaseCommand.RepaymentAmount, leaseCommand.InterestRate, leaseCommand.PaymentsToDate, leaseCommand.Purpose, leaseCommand.ValueOfAssets, leaseCommand.ResponsibleForAssets);
			
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
