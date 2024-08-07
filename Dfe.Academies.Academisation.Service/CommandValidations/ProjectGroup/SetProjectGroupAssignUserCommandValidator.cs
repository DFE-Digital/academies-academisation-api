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
				.NotNull().WithMessage("Group Reference number must not be null");
			/*
			RuleFor(x => x.FullName)
				.NotEmpty()
				.When(x => x.UserId != null && !string.IsNullOrEmpty(x.EmailAddress))
				.WithMessage("Full name must not be empty");

			RuleFor(x => x.UserId)
				.NotEmpty()
				.When(x => !string.IsNullOrEmpty(x.FullName) && !string.IsNullOrEmpty(x.EmailAddress))
				.WithMessage("Full name must not be empty");

			RuleFor(x => x.EmailAddress)
				.NotEmpty()
				.When(x => x.UserId != null && !string.IsNullOrEmpty(x.FullName))
				.EmailAddress().WithMessage("Must be a valid email address.");*/
		}
	}
}
