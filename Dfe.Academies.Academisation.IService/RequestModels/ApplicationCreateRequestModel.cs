namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateRequestModel
{
	public ApplicationCreateRequestModel(int applicationType, List<ContributorDetailsRequestModel> contributors)
	{
		ApplicationType = applicationType;
		Contributors = contributors;
	}

	public int ApplicationType { get; }
	public IReadOnlyCollection<ContributorDetailsRequestModel> Contributors { get; }
}
