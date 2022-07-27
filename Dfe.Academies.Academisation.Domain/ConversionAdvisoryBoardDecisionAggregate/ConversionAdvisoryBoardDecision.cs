using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecision : IConversionAdvisoryBoardDecision
{
	private ConversionAdvisoryBoardDecision(AdvisoryBoardDecisionDetails details)
	{
		AdvisoryBoardDecisionDetails = details;
	}

	private static readonly CreateConversionAdvisoryBoardDecisionValidator CreateConversionAdvisoryBoardDecisionValidator  = new();
	
	public AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }
	public int Id { get; set; }
	
	internal static CreateResult<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details)
	{
		var validationResult = CreateConversionAdvisoryBoardDecisionValidator.Validate(details);

		return validationResult.IsValid
			? new CreateSuccessResult<IConversionAdvisoryBoardDecision>(
				new ConversionAdvisoryBoardDecision(details))
			: new CreateValidationErrorResult<IConversionAdvisoryBoardDecision>(
				validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
	}	
}
