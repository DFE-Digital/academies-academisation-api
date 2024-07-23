using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup
{
	public class CreateProjectGroupCommandValidator : AbstractValidator<CreateProjectGroupCommand>
	{
		public CreateProjectGroupCommandValidator()
		{
			RuleFor(x => x.TrustUrn)
				.NotEmpty().WithMessage("Must specify a trust reference")
				.Length(8).WithMessage("Trust reference must be length 8")
				.NotNull().WithMessage("Trust Reference must not be null");
		}
	}
}
