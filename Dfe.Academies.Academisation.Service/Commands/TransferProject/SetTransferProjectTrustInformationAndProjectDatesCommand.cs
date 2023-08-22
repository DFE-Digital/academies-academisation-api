using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

public class SetTransferProjectTrustInformationAndProjectDatesCommand : SetTransferProjectCommand
{
	public string Recommendation { get; set; }
	public string Author { get; set; }
}
