using Dfe.Academies.Academisation.Core;
using MediatR;

public class SetDeletedAtCommand : IRequest<CommandResult>
{
	public SetDeletedAtCommand(int projectId)
	{
		ProjectId = projectId;
	}

	public int ProjectId { get; set; }
}

