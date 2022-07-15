using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateRequestModel
{
	public ApplicationCreateRequestModel(ApplicationType applicationType, List<ContributorDetailsRequestModel> contributors)
	{
		ApplicationType = applicationType;
		Contributors = contributors;
	}

	public ApplicationType ApplicationType { get; }
	public IReadOnlyCollection<ContributorDetailsRequestModel> Contributors { get; }
}
