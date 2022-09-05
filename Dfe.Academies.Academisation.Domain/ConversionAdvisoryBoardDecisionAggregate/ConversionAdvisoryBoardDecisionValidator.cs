using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionValidator : AbstractValidator<AdvisoryBoardDecisionDetails>
{
	public ConversionAdvisoryBoardDecisionValidator()
	{
		ValidateApprovedDecision();
		ValidateDeclinedDecision();
		ValidateDeferredDecision();
		ValidateDecisionDate();
	}

	private static string NotNullMessage(string target)
	{
		return $"{target} should not be null";
	}

	private static string NullMessage(string target)
	{
		return $"{target} should be null";
	}

	private static string WhenIsNotSuffix(string target, string value)
	{
		return $" when {target} is not {value}";
	}

	private static string WhenIsSuffix(string target, string value)
	{
		return $" when {target} is {value}";
	}

	private static string NotEmptyMessage(string target)
	{
		return $"{target} should not be null or empty";
	}

	private static string EmptyMessage(string target)
	{
		return $"{target} should be null or empty";
	}

	private static string AndSuffix(string conditional, string value)
	{
		return $" and {conditional} is {value}";
	}

	private static string OrSuffix(string conditional, string value)
	{
		return $" or {conditional} is {value}";
	}

	private static string InNestedPropertySuffix(string collection, string index)
	{
		return $" for element {index} in {collection}";
	}

	private static string NotSetDateMessage(string target)
	{
		return $"{target} must be set to a valid date";
	}

	private static string FutureDateMessage(string target)
	{
		return $"{target} must not be in the future";
	}

	private void ValidateApprovedDecision()
	{
		RuleFor(details => details.ApprovedConditionsSet)
			.NotNull()
			.When(details => details.Decision is AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NotNullMessage(
					nameof(details.ApprovedConditionsSet)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.ApprovedConditionsSet)
			.Null()
			.When(details => details.Decision is not AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NullMessage(
					nameof(details.ApprovedConditionsSet)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.ApprovedConditionsDetails)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecision.Approved &&
				details.ApprovedConditionsSet is true)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				AndSuffix(
					nameof(details.ApprovedConditionsSet),
					bool.TrueString));

		RuleFor(details => details.ApprovedConditionsDetails)
			.Empty()
			.When(details =>
				details.Decision is not AdvisoryBoardDecision.Approved ||
				details.ApprovedConditionsSet is false)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				OrSuffix(
					nameof(details.ApprovedConditionsSet),
					bool.FalseString));
	}

	private void ValidateDeclinedDecision()
	{
		RuleFor(details => details.DeclinedReasons)
			.NotEmpty()
			.When(details => details.Decision is AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleFor(details => details.DeclinedReasons)
			.Empty()
			.When(details => details.Decision is not AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleForEach(details => details.DeclinedReasons)
			.ChildRules(reason => reason
				.RuleFor(r => r.Details)
				.NotEmpty()
				.WithMessage(r =>
					NotEmptyMessage(
						nameof(r.Details)) +
					InNestedPropertySuffix(
						nameof(AdvisoryBoardDecisionDetails.DeferredReasons),
						"{CollectionIndex}")));
	}

	private void ValidateDeferredDecision()
	{
		RuleFor(details => details.DeferredReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.Decision is AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredReasons)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Deferred)));

		RuleFor(details => details.DeferredReasons)
			.Empty()
			.When(details => details.Decision is not AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeferredReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleForEach(details => details.DeferredReasons)
			.ChildRules(reason => reason
				.RuleFor(r => r.Details)
				.NotEmpty()
				.WithMessage(r =>
					NotEmptyMessage(
						nameof(r.Details)) +
					InNestedPropertySuffix(
						nameof(AdvisoryBoardDecisionDetails.DeferredReasons),
						"{CollectionIndex}")));
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
