using Dfe.Academies.Academisation.Core;
using MediatR;

public class SetFormAMatProjectReferenceCommand : IRequest<CommandResult>
{
	public SetFormAMatProjectReferenceCommand(int projectId, int formAMatProjectId)
	{
		ProjectId = projectId;
		FormAMatProjectId = formAMatProjectId;
	}

	public int ProjectId { get; }
	public int FormAMatProjectId { get; }
}

