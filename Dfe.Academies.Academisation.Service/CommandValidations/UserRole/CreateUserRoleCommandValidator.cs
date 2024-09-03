using Dfe.Academies.Academisation.Service.Commands.UserRole;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.UserRole
{
	public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
	{
		public CreateUserRoleCommandValidator()
		{
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Full name must not be empty")
				.NotNull().WithMessage("Full name must not be null"); ;

			RuleFor(x => x.UserId)
				.NotNull().WithMessage("User Id cannot be null.")
				.NotEmpty().WithMessage("User Id cannot be an empty GUID.");

			RuleFor(x => x.EmailAddress)
				.NotNull().WithMessage("Email address cannot be null.")
				.NotEmpty().WithMessage("Email address cannot be empty.")
				.EmailAddress().WithMessage("Email address must be a valid email address.");

			RuleFor(x => x.RoleId)
				.IsInEnum().WithMessage("Role Id must be a valid value.");
		}
	}
}
