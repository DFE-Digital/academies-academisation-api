namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDeferredReasonDetails(int Id, int AdvisoryBoardDecisionId, AdvisoryBoardDeferredReason Reason, string Details);
