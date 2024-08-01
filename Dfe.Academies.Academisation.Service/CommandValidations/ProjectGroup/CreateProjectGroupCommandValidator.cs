using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup
{
	public class CreateProjectGroupCommandValidator : AbstractValidator<CreateProjectGroupCommand>
	{
		public CreateProjectGroupCommandValidator()
		{
			RuleFor(x => x.TrustReferenceNumber)
				.NotEmpty().WithMessage("Must specify a trust reference")
				.Length(7).WithMessage("Trust reference must be length 7")
				.NotNull().WithMessage("Trust Reference must not be null");
		}
	}
}
