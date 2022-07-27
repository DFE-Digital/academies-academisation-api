using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateRequestModel
{
	public ApplicationCreateRequestModel(ApplicationType applicationType, ContributorDetailsRequestModel contributor)
	{
		ApplicationType = applicationType;
		Contributor = contributor;
	}

	public ApplicationType ApplicationType { get; }
	public ContributorDetailsRequestModel Contributor { get; }
}
