using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations;

internal class DeleteApplicationValidator : AbstractValidator<int>
{
	public DeleteApplicationValidator()
	{
		RuleFor(x => x).
		NotEmpty().WithMessage("Application Id cannot be empty")
		.OverridePropertyName("Appliction Id");
	}
}
