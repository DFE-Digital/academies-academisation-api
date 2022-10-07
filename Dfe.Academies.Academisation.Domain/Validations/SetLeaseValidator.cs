using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations
{
	internal class SetLeaseValidator : AbstractValidator<Lease>
	{
		public SetLeaseValidator()
		{
			RuleFor(x => x.Purpose).NotEmpty().WithMessage("Must specify a purpose");
			RuleFor(x => x.InterestRate).GreaterThan(0).WithMessage("Must have a positive interest rate");
			RuleFor(x => x.LeaseTerm).GreaterThan(0).WithMessage("Must a positive lease term");
			RuleFor(x => x.RepaymentAmount).GreaterThan(0).WithMessage("Must have a positive repayment amount");
			RuleFor(x => x.PaymentsToDate).GreaterThan(0).WithMessage("Must specify any payments to date");
			RuleFor(x => x.ResponsibleForAssets).NotEmpty().WithMessage("Must state who is responsible for the assets");
			RuleFor(x => x.ValueOfAssets).NotEmpty().WithMessage("Must specify the value of the assets");
		}
	}
}
