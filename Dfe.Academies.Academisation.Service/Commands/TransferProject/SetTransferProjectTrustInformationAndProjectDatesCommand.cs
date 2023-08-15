using Dfe.Academies.Academisation.Core;
using MediatR;

public class SetTransferProjectTrustInformationAndProjectDatesCommand : IRequest<CommandResult>
{
	public int Id { get; set; }
	public string Recommendation { get; set; }
	public string Author { get; set; }
}
