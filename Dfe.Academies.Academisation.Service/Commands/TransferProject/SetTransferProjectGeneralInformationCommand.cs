using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectGeneralInformationCommand : SetTransferProjectCommand
	{
		public string Recommendation { get; set; }
		public string Author { get; set; }
	}
}
