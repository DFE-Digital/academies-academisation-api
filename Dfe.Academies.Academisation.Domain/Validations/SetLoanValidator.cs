using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations
{
	internal class SetLoanValidator : AbstractValidator<Loan>
	{
		public SetLoanValidator()
		{
			RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
			RuleFor(x => x.Purpose).NotEmpty().WithMessage("Must have a purpose");
			RuleFor(x => x.Provider).NotEmpty().WithMessage("Must state a provider");
			RuleFor(x => x.InterestRate).GreaterThan(0).WithMessage("Must have a positive interest rate");
			RuleFor(x => x.Schedule).NotEmpty().WithMessage("Must specify a schedule");
		}
	}
}
