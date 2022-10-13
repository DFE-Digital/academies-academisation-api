using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.CommandValidations;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class UpdateLeaseCommandHandler : IUpdateLeaseCommandHandler
{
	private readonly IApplicationRepository _applicationRepository;
	private readonly IValidatorFactory<UpdateLeaseCommand> _validatorFactory;
	public UpdateLeaseCommandHandler(IApplicationRepository applicationRepository, IValidatorFactory<UpdateLeaseCommand> validatorFactory)
	{
		_applicationRepository = applicationRepository;
		_validatorFactory = validatorFactory;
	}

	public async Task<CommandResult> Handle(UpdateLeaseCommand leaseCommand)
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
			
		var result = existingApplication.UpdateLease(leaseCommand.SchoolId,  leaseCommand.LeaseId, leaseCommand.LeaseTerm, leaseCommand.RepaymentAmount, leaseCommand.InterestRate, leaseCommand.PaymentsToDate, leaseCommand.Purpose, leaseCommand.ValueOfAssets, leaseCommand.ResponsibleForAssets);
			
		if (result is CommandValidationErrorResult)
		{
			return result;
		}
		if (result is NotFoundCommandResult)
		{
			throw new NotFoundException($"Lease not found: {leaseCommand.LeaseId}", "Lease");
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
