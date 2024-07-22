using System.Runtime.Serialization;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommand : IRequest<CommandResult>
	{
		public int Urn { get; set; }
		public required string TrustReference { get; set; }
		public required string ReferenceNumber { get; set; }
	}
}
