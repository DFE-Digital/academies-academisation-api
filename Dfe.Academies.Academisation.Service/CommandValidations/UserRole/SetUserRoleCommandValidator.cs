using Dfe.Academies.Academisation.Service.Commands.UserRole;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.UserRole
{
	public class SetUserRoleCommandValidator : AbstractValidator<SetUserRoleCommand>
	{
		public SetUserRoleCommandValidator()
		{
			RuleFor(x => x.EmailAddress)
				.NotNull().WithMessage("Email address cannot be null.")
				.NotEmpty().WithMessage("Email address cannot be empty.")
				.EmailAddress().WithMessage("Email address must be a valid email address.");

			RuleFor(x => x.RoleId)
				.IsInEnum().WithMessage("Role Id must be a valid value.");
		}
	}
}
