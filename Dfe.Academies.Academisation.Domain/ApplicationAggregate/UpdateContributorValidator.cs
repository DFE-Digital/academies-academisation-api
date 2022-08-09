using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	internal class UpdateContributorValidator : AbstractValidator<ContributorPair>
	{
		public UpdateContributorValidator()
		{
			RuleFor(x => x)
				.Must(x => x.Updated.EmailAddress == x.Existing.EmailAddress)
				.WithMessage("Cannot change the email address of a contributor, add a new contributor")
				.OverridePropertyName(nameof(ContributorDetails.EmailAddress));
		}
	}

	internal record ContributorPair(ContributorDetails Updated, ContributorDetails Existing);
}
