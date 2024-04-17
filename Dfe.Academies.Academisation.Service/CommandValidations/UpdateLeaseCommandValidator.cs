using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations;

public class UpdateLeaseCommandValidator : AbstractValidator<UpdateLeaseCommand>
{
	public UpdateLeaseCommandValidator()
	{
		RuleFor(x => x.LeaseId).NotNull().WithMessage("Must be a valid Lease ID");
		RuleFor(x => x.Purpose).NotEmpty().WithMessage("Must specify a purpose");
		RuleFor(x => x.InterestRate).GreaterThan(0).WithMessage("Must have a positive interest rate");
		RuleFor(x => x.LeaseTerm).NotEmpty().WithMessage("Must specify the lease terms");
		RuleFor(x => x.RepaymentAmount).GreaterThan(0).WithMessage("Must have a positive repayment amount");
		RuleFor(x => x.PaymentsToDate).GreaterThan(0).WithMessage("Must specify any payments to date");
		RuleFor(x => x.ResponsibleForAssets).NotEmpty().WithMessage("Must state who is responsible for the assets");
		RuleFor(x => x.ValueOfAssets).NotEmpty().WithMessage("Must specify the value of the assets");
	}
}
