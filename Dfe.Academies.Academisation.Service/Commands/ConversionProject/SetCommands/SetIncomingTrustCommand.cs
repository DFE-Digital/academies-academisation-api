using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetIncomingTrustCommand : IRequest<CommandResult>
	{
		public SetIncomingTrustCommand(int id,
			string trustReferrenceNumber,
			string trustName)
		{
			Id = id;
			TrustReferrenceNumber = trustReferrenceNumber;
			TrustName = trustName;
		}

		public int Id { get; set; }
		public string TrustReferrenceNumber { get; set; }
		public string TrustName { get; set; }
	}
}
