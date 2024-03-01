namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDeclinedReasonDetails(int Id, int AdvisoryBoardDecisionId, AdvisoryBoardDeclinedReason Reason, string Details);
