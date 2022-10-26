using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations
{
	internal class SubmitApplicationValidator : AbstractValidator<Application>
	{
		public SubmitApplicationValidator()
		{
			RuleFor(application => application.ApplicationStatus)
				.Equal(ApplicationStatus.InProgress)
				.WithMessage("Application must be In Progress to submit");

			RuleFor(application => application.Schools)
				.Must(schools => schools.Any())
				.WithMessage("Application must have at least one School to submit");

			RuleFor(application => application)
				.SetValidator(new SubmitJoinAMatFormASatApplicationValidator())
				.When(application => 
					application.ApplicationType == ApplicationType.JoinAMat
					|| application.ApplicationType == ApplicationType.FormASat);
		}
	}
}
