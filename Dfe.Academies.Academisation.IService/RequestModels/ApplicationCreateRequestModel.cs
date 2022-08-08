using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateRequestModel
{
	public ApplicationCreateRequestModel(ApplicationType applicationType, ContributorRequestModel contributor)
	{
		ApplicationType = applicationType;
		Contributor = contributor;
	}

	public ApplicationType ApplicationType { get; }
	public ContributorRequestModel Contributor { get; }
}
