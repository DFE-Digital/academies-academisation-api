using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public class AdvisoryBoardDecision : IAdvisoryBoardDecision
{
	public int ProjectId { get; }
	public int Id { get; set; }
	public IAdvisoryBoardDecisionDetails Details { get; }

	internal AdvisoryBoardDecision(int projectId, IAdvisoryBoardDecisionDetails details)
	{
		ProjectId = projectId;
		Details = details;
	}
}