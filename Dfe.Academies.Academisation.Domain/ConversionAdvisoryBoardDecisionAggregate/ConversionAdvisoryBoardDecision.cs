using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using ValidationException = FluentValidation.ValidationException;

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
	
	internal static async Task<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details)
	{
		var validationResult = await CreateConversionAdvisoryBoardDecisionValidator.ValidateAsync(details);

		if (!validationResult.IsValid) throw new ValidationException(validationResult.ToString());

		return new ConversionAdvisoryBoardDecision(details);
	}	
}
