﻿using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "AdvisoryBoardDecisionDeferredReason")]
public class AdvisoryBoardDecisionDeferredReasonState : BaseEntity
{
	public int AdvisoryBoardDecisionId { get; set; }
	public AdvisoryBoardDeferredReason Reason { get; init; }
	public string Details { get; init; } = null!;
}
