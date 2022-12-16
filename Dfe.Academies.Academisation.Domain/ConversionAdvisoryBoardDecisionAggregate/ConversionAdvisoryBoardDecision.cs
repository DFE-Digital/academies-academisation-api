using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecision : IConversionAdvisoryBoardDecision
{
	private ConversionAdvisoryBoardDecision(AdvisoryBoardDecisionDetails details)
	{
		AdvisoryBoardDecisionDetails = details;
	}

	public ConversionAdvisoryBoardDecision(
		int id,
		AdvisoryBoardDecisionDetails details,
		DateTime createdOn,
		DateTime lastModifiedOn) : this(details)
	{
		Id = id;
		CreatedOn = createdOn;
		LastModifiedOn = lastModifiedOn;
	}

	private static readonly ConversionAdvisoryBoardDecisionValidator Validator = new();

	public AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; private set; }
	public int Id { get; private set; }
	public DateTime CreatedOn { get; }
	public DateTime LastModifiedOn { get; }

	internal static CreateResult Create(AdvisoryBoardDecisionDetails details)
	{
		var validationResult = Validator.Validate(details);

		return validationResult.IsValid
			? new CreateSuccessResult<IConversionAdvisoryBoardDecision>(
				new ConversionAdvisoryBoardDecision(details))
			: new CreateValidationErrorResult(
				validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
	}

	public CommandResult Update(AdvisoryBoardDecisionDetails details)
	{
		var validationResult = Validator.Validate(details);

		if (!validationResult.IsValid)
		{
			var validationError = validationResult.Errors
				.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage));

			return new CommandValidationErrorResult(validationError);
		}

		AdvisoryBoardDecisionDetails = details;
		return new CommandSuccessResult();
	}

	public void SetId(int id)
	{
		Id = Id == default
		? id
		: throw new InvalidOperationException("Cannot assign an id when the id has already been set");
	}
}
