using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	internal class SubmitApplicationValidator : AbstractValidator<Application>
	{
		public SubmitApplicationValidator()
		{
			RuleFor(application => application.ApplicationStatus)
				.Equal(ApplicationStatus.InProgress)
				.WithMessage("Application must be In Progress to submit");
		}
	}
}
