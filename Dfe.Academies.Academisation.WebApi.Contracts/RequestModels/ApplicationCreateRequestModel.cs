namespace Dfe.Academies.Academisation.WebApi.Contracts.RequestModels;
using Dfe.Academies.Academisation.WebApi.Contracts.FromDomain;

public class ApplicationCreateRequestModel
{
	public ApplicationCreateRequestModel(ApplicationType applicationType, ContributorRequestModel contributor)
	{
		ApplicationType = applicationType;
		Contributor = contributor;
	}

	public ApplicationType ApplicationType { set; get; }
	public ContributorRequestModel Contributor { set; get; }
}
