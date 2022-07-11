using Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.AdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecision : IAdvisoryBoardDecision
{
    private static readonly CreateAdvisoryBoardDecisionValidator CreateValidator = new();
    
    public int Id { get; set; }
    public IAdvisoryBoardDecisionDetails Details { get; }
    
    private AdvisoryBoardDecision(IAdvisoryBoardDecisionDetails details)
    {
        Details = details;
    }

    internal static async Task<AdvisoryBoardDecision> Create(IAdvisoryBoardDecisionDetails details)
    {
        var validationResult = await CreateValidator.ValidateAsync(details);

        if (validationResult.IsValid)
        {
            return new(details);
        }

        throw new ValidationException(validationResult.ToString());
    }
    
}