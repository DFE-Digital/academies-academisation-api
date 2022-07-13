using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public class AdvisoryBoardDecision : IAdvisoryBoardDecision
{
    private static readonly CreateAdvisoryBoardDecisionValidator CreateValidator = new();
    
    public int ProjectId { get; }
    public int Id { get; set; }
    public IAdvisoryBoardDecisionDetails Details { get; }
    
    private AdvisoryBoardDecision(int projectId, IAdvisoryBoardDecisionDetails details)
    {
        ProjectId = projectId;
        Details = details;
    }

    internal static async Task<AdvisoryBoardDecision> Create(int projectId, IAdvisoryBoardDecisionDetails details)
    {
        var validationResult = await CreateValidator.ValidateAsync(details);

        if (validationResult.IsValid)
        {
            return new(projectId, details);
        }

        throw new ValidationException(validationResult.ToString());
    }
    
}