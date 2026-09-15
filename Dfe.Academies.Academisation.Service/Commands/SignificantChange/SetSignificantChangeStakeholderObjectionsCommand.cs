using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeStakeholderObjectionsPublicCommand(
	SignificantChangeStakeholderObjections? stakeholderObjections,
	string? stakeholderObjectionsComment) : IRequest<CommandResult>
{
	public SignificantChangeStakeholderObjections? StakeholderObjections { get; set; } = stakeholderObjections;
	public string? StakeholderObjectionsComment { get; set; } = stakeholderObjectionsComment;
}

public class SetSignificantChangeStakeholderObjectionsCommand(
	int id,
	SignificantChangeStakeholderObjections? stakeholderObjections,
	string? stakeholderObjectionsComment)
	: SetSignificantChangeStakeholderObjectionsPublicCommand(stakeholderObjections, stakeholderObjectionsComment)
{
	public int Id { get; set; } = id;
}