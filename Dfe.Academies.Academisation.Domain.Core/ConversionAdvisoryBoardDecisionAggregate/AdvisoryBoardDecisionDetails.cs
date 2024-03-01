namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDecisionDetails(
	int? ConversionProjectId,
	int? TransferProjectId,
	AdvisoryBoardDecision Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	DateTime AdvisoryBoardDecisionDate,
	DecisionMadeBy DecisionMadeBy
);
