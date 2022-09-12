using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	internal class SubmitJoinAMatApplicationValidator : AbstractValidator<Application>
	{
		public SubmitJoinAMatApplicationValidator()
		{
			RuleFor(application => application.Schools)
				.Must(schools => schools.Count < 2)
				.WithMessage("Join a MAT Applications must have one and only one School");
		}
	}
}
