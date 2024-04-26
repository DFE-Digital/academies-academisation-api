using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionValidator : AbstractValidator<ConversionAdvisoryBoardDecision>
{
	public ConversionAdvisoryBoardDecisionValidator()
	{
		ValidateApprovedDecision();
		ValidateDeclinedDecision();
		ValidateDeferredDecision();
		ValidateWithdrawnDecision();
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
		RuleFor(details => details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)
			.NotNull()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NotNullMessage(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)) +
				WhenIsSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)
			.Null()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is not AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NullMessage(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)) +
				WhenIsNotSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)
			.NotEmpty()
			.When(details =>
				details.AdvisoryBoardDecisionDetails.Decision is AdvisoryBoardDecision.Approved &&
				details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet is true)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)) +
				WhenIsSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				AndSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet),
					bool.TrueString));

		RuleFor(details => details.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)
			.Empty()
			.When(details =>
				details.AdvisoryBoardDecisionDetails.Decision is not AdvisoryBoardDecision.Approved ||
				details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet is false)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)) +
				WhenIsNotSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				OrSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.ApprovedConditionsSet),
					bool.FalseString));
	}

	private void ValidateDeclinedDecision()
	{
		RuleFor(details => details.DeclinedReasons)
			.NotEmpty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleFor(details => details.DeclinedReasons)
			.Empty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is not AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsNotSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleForEach(details => details.DeclinedReasons)
			.ChildRules(reason => reason
				.RuleFor(r => r.Details)
				.NotEmpty()
				.WithMessage(r =>
					NotEmptyMessage(
						nameof(r.Details)) +
					InNestedPropertySuffix(
						nameof(ConversionAdvisoryBoardDecision.DeclinedReasons),
						"{CollectionIndex}")));
	}

	private void ValidateDeferredDecision()
	{
		RuleFor(details => details.DeferredReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredReasons)) +
				WhenIsSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Deferred)));

		RuleFor(details => details.DeferredReasons)
			.Empty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is not AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeferredReasons)) +
				WhenIsNotSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleForEach(details => details.DeferredReasons)
			.ChildRules(reason => reason
				.RuleFor(r => r.Details)
				.NotEmpty()
				.WithMessage(r =>
					NotEmptyMessage(
						nameof(r.Details)) +
					InNestedPropertySuffix(
						nameof(ConversionAdvisoryBoardDecision.DeferredReasons),
						"{CollectionIndex}")));
	}

	private void ValidateWithdrawnDecision()
	{
		RuleFor(details => details.WithdrawnReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is AdvisoryBoardDecision.Withdrawn)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.WithdrawnReasons)) +
				WhenIsSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Withdrawn)));

		RuleFor(details => details.WithdrawnReasons)
			.Empty()
			.When(details => details.AdvisoryBoardDecisionDetails.Decision is not AdvisoryBoardDecision.Withdrawn)
			.WithMessage(details =>
				NullMessage(
					nameof(details.WithdrawnReasons)) +
				WhenIsNotSuffix(
					nameof(details.AdvisoryBoardDecisionDetails.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleForEach(details => details.WithdrawnReasons)
			.ChildRules(reason => reason
				.RuleFor(r => r.Details)
				.NotEmpty()
				.WithMessage(r =>
					NotEmptyMessage(
						nameof(r.Details)) +
					InNestedPropertySuffix(
						nameof(ConversionAdvisoryBoardDecision.WithdrawnReasons),
						"{CollectionIndex}")));
	}


	private void ValidateDecisionDate()
	{
		RuleFor(details => details.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)
			.Must(date => date != default)
			.WithMessage(details =>
				NotSetDateMessage(
					nameof(details.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)));

		RuleFor(details => details.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)
			.Must(date => date < DateTime.UtcNow)
			.WithMessage(details =>
				FutureDateMessage(
					nameof(details.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)));
	}
}
