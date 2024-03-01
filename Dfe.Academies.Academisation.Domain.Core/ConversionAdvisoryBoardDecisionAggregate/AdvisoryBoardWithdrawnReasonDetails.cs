namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardWithdrawnReasonDetails(int Id, int AdvisoryBoardDecisionId, AdvisoryBoardWithdrawnReason Reason, string Details);
