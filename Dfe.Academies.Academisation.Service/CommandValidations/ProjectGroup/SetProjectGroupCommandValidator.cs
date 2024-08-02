using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup
{
	public class SetProjectGroupCommandValidator : AbstractValidator<SetProjectGroupCommand>
	{
		public SetProjectGroupCommandValidator()
		{
			RuleFor(x => x.GroupReferenceNumber)
				.NotEmpty().WithMessage("Must specify a group reference number")
				.NotNull().WithMessage("Trust Reference must not be null");
		}
	}
}
