using MediatR;
using Dfe.Academies.Academisation.Core;
using System.Runtime.Serialization;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustReference, List<int> conversionProjects) : IRequest<CommandResult>
	{
		[DataMember]
		public string TrustReference { get; set; } = trustReference;

		[DataMember]
		public List<int> ConversionProjectsUrns { get; set; } = conversionProjects;
	}
}
