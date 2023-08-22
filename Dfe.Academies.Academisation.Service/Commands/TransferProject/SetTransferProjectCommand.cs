using Dfe.Academies.Academisation.Core;
using MediatR;

namespace TramsDataApi.RequestModels.AcademyTransferProject
{
    public abstract class SetTransferProjectCommand : IRequest<CommandResult>
    {
		public int Urn { get; set; }
    }
}
