using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup
{
	public class SetProjectGroupAssignUserCommandValidator : AbstractValidator<SetProjectGroupAssignUserCommand>
	{
		public SetProjectGroupAssignUserCommandValidator()
		{
			RuleFor(x => x.GroupReferenceNumber)
				.NotEmpty().WithMessage("Must specify a group reference number")
				.NotNull().WithMessage("Trust Reference must not be null");

			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Full name must not be empty");

			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("Full name must not be empty");

			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Full name must not be empty");

			RuleFor(x => x.EmailAddress)
				.NotEmpty().WithMessage("Email address must not be empty")
				.EmailAddress().WithMessage("Must be a valid email address.");
		}
	}
}
