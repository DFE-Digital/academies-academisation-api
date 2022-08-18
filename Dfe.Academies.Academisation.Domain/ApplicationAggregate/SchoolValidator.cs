using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	internal class SchoolValidator : AbstractValidator<SchoolDetails>
	{
		public SchoolValidator()
		{
			RuleFor(x => x.ApproverContactEmail)
				.EmailAddress();
			RuleFor(x => x.ContactChairEmail)
				.EmailAddress();
			RuleFor(x => x.ContactHeadEmail)
				.EmailAddress();
			RuleFor(x => x.MainContactOtherEmail)
				.EmailAddress();
			RuleFor(x => x.SchoolName)
				.NotEmpty();
		}
	}
}
