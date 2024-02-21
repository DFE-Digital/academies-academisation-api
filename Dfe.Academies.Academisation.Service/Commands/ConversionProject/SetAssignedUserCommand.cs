using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class SetAssignedUserCommand : IRequest<CommandResult>
	{
		public SetAssignedUserCommand(
			int id,
			Guid userId,
			string fullName,
			string emailAddress)
		{
			Id = id;
			UserId = userId;
			FullName = fullName;
			EmailAddress = emailAddress;
		}

		public int Id { get; set; }
		public Guid UserId { get; set; }
		public string FullName { get; set; }

		public string EmailAddress { get; set; }
	}
}


