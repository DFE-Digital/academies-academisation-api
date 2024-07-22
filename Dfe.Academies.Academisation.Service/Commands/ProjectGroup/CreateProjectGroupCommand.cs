using MediatR;
using Dfe.Academies.Academisation.Core;
using System.Runtime.Serialization;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustReference, string referenceNumber) : IRequest<CommandResult>
	{
		[DataMember]
		public string TrustReference { get; set; } = trustReference;
		[DataMember]
		public string ReferenceNumber { get; set; } = referenceNumber;
	}
}
