using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeAssignedUserPublicCommand(
	Guid userId,
	string fullName,
	string emailAddress) : IRequest<CommandResult>
{
	public Guid UserId { get; set; } = userId;
	public string FullName { get; set; } = fullName;
	public string EmailAddress { get; set; } = emailAddress;
}

public class SetSignificantChangeAssignedUserCommand(
	int id,
	Guid userId,
	string fullName,
	string emailAddress) : SetSignificantChangeAssignedUserPublicCommand(userId, fullName, emailAddress)
{
	public int Id { get; set; } = id;
}
