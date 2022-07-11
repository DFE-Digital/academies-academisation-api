using Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.AdvisoryBoardDecisionAggregate;

internal class CreateAdvisoryBoardDecisionValidator : AbstractValidator<IAdvisoryBoardDecisionDetails>
{
	public CreateAdvisoryBoardDecisionValidator()
	{
		ValidateApprovedDecision();
		ValidateDeclinedDecision();
		ValidateDeferredDecision();
		ValidateDecisionDate();
	}

	private static string NotNullMessage(string target) => $"{target} should not be null";
	private static string NullMessage(string target) => $"{target} should be null";
	private static string WhenIsNotSuffix(string target, string value) => $" when {target} is not {value}";
	private static string WhenIsSuffix(string target, string value) => $" when {target} is {value}";
	private static string NotEmptyMessage(string target) => $"{target} should not be null or empty";
	private static string EmptyMessage(string target) => $"{target} should be null or empty";
	private static string AndSuffix(string conditional, string value) => $" and {conditional} is {value}";
	private static string OrSuffix(string conditional, string value) => $" or {conditional} is {value}";
	private static string AndContainsSuffix(string target, string value) => $" and {target} contains {value}";
	private static string AndNotContainsSuffix(string target, string value) => $" and {target} does not contain {value}";
	private static string OrNotContainsSuffix(string target, string value) => $" or {target} does not contain {value}";
	private static string NotSetDateMessage(string target) => $"{target} must be set to a valid date";
	private static string FutureDateMessage(string target) => $"{target} must not be in the future";
	
	private void ValidateApprovedDecision()
	{
		RuleFor(details => details.ApprovedConditionsSet)
			.NotNull()
			.When(details => details.Decision is AdvisoryBoardDecisions.Approved)
			.WithMessage(details =>
				NotNullMessage(
					nameof(details.ApprovedConditionsSet)) +
				WhenIsSuffix(
					nameof(details.Decision), 
					nameof(AdvisoryBoardDecisions.Approved)));
				
		RuleFor(details => details.ApprovedConditionsSet)
			.Null()
			.When(details => details.Decision is not AdvisoryBoardDecisions.Approved)
			.WithMessage(details =>
				NullMessage(
					nameof(details.ApprovedConditionsSet)) +
			    WhenIsNotSuffix(
				    nameof(details.Decision), 
				    nameof(AdvisoryBoardDecisions.Approved)));
		
		RuleFor(details => details.ApprovedConditionsDetails)
			.NotEmpty()
			.When(details => 
				details.Decision is AdvisoryBoardDecisions.Approved && 
				details.ApprovedConditionsSet is true)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsSuffix(
					nameof(details.Decision), 
					nameof(AdvisoryBoardDecisions.Approved)) +
				AndSuffix(
					nameof(details.ApprovedConditionsSet),
					bool.TrueString));
		
		RuleFor(details => details.ApprovedConditionsDetails)
			.Empty()
			.When(details => 
				details.Decision is not AdvisoryBoardDecisions.Approved || 
				details.ApprovedConditionsSet is false)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Approved)) + 
				OrSuffix(
					nameof(details.ApprovedConditionsSet), 
					bool.FalseString));
	}

	private void ValidateDeclinedDecision()
	{
		RuleFor(details => details.DeclinedReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.Decision is AdvisoryBoardDecisions.Declined)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsSuffix(
					nameof(details.Decision), 
					nameof(AdvisoryBoardDecisions.Declined)));

		RuleFor(details => details.DeclinedReasons)
			.Null()
			.When(details => details.Decision is not AdvisoryBoardDecisions.Declined)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision), 
					nameof(AdvisoryBoardDecisions.Declined)));

		RuleFor(details => details.DeclinedOtherReason)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecisions.Declined &&
				details.DeclinedReasons is not null &&
				details.DeclinedReasons.Contains(AdvisoryBoardDeclinedReasons.Other))
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedOtherReason)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Declined)) +
				AndContainsSuffix(
					nameof(details.DeclinedReasons),
					nameof(AdvisoryBoardDeclinedReasons.Other)));

		RuleFor(details => details.DeclinedOtherReason)
			.Null()
			.When(details => 
				details.Decision is not AdvisoryBoardDecisions.Declined ||
				(details.DeclinedReasons is not null &&
				!details.DeclinedReasons.Contains(AdvisoryBoardDeclinedReasons.Other)))
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeclinedOtherReason)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Declined)) +
				OrNotContainsSuffix(
					nameof(details.DeclinedReasons), 
					nameof(AdvisoryBoardDeclinedReasons.Other)));
	}

	private void ValidateDeferredDecision()
	{
		RuleFor(details => details.DeferredReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.Decision is AdvisoryBoardDecisions.Deferred)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredReasons)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Deferred)));

		RuleFor(details => details.DeferredReasons)
			.Null()
			.When(details => details.Decision is not AdvisoryBoardDecisions.Deferred)
			.WithMessage(details => 
				NullMessage(
					nameof(details.DeferredReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision), 
					nameof(AdvisoryBoardDecisions.Deferred)));

		RuleFor(details => details.DeferredOtherReason)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecisions.Deferred &&
				details.DeferredReasons is not null &&
				details.DeferredReasons!.Contains(AdvisoryBoardDeferredReasons.Other))
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredOtherReason)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Deferred)) +
				AndContainsSuffix(
					nameof(details.DeferredReasons),
					nameof(AdvisoryBoardDeferredReasons.Other)));

		RuleFor(details => details.DeferredOtherReason)
			.Null()
			.When(details =>
				details.Decision is not AdvisoryBoardDecisions.Deferred ||
				(details.DeferredReasons is not null &&
				!details.DeferredReasons.Contains(AdvisoryBoardDeferredReasons.Other)))
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeferredOtherReason)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecisions.Deferred)) +
				AndNotContainsSuffix(
					nameof(details.DeferredReasons),
					nameof(AdvisoryBoardDeferredReasons.Other)));
	}

	private void ValidateDecisionDate()
	{
		RuleFor(details => details.AdvisoryBoardDecisionDate)
			.Must(date => date != default)
			.WithMessage(details =>
				NotSetDateMessage(
					nameof(details.AdvisoryBoardDecisionDate)));

		RuleFor(details => details.AdvisoryBoardDecisionDate)
			.Must(date => date < DateTime.UtcNow)
			.WithMessage(details =>
				FutureDateMessage(
					nameof(details.AdvisoryBoardDecisionDate)));
	}
}
