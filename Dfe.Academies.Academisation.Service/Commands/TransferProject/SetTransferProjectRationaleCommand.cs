using Dfe.Academies.Academisation.Core;
using MediatR;

namespace TramsDataApi.RequestModels.AcademyTransferProject
{
    public class SetTransferProjectRationaleCommand : SetTransferProjectCommand
	{
        public string ProjectRationale { get; set; }
        public string TrustSponsorRationale { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
