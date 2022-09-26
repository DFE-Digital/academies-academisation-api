using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	internal class SubmitJoinAMatFormASatApplicationValidator : AbstractValidator<Application>
	{
		public SubmitJoinAMatFormASatApplicationValidator()
		{
			RuleFor(application => application.Schools)
				.Must(schools => schools.Count < 2)
				.WithMessage("Join a MAT and form a SAT Applications must have one and only one School");
		}
	}
}
