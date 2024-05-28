using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectDeletedAtCommand : SetTransferProjectCommand
	{
		public SetTransferProjectDeletedAtCommand(int urn)
		{
			Urn = urn;
		}

		public int Urn { get; set; }
	}
}
