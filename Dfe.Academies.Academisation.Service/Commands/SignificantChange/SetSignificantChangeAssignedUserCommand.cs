using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeAssignedUserCommand(
	int id,
	Guid userId,
	string fullName,
	string emailAddress) : IRequest<CommandResult>
{
	public int Id { get; set; } = id;
	public Guid UserId { get; set; } = userId;
	public string FullName { get; set; } = fullName;
	public string EmailAddress { get; set; } = emailAddress;
}