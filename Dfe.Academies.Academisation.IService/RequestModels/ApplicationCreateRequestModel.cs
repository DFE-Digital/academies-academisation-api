namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateRequestModel
{
	public int ApplicationType { get; }
	List<ContributorDetailsRequestModel> Contributors { get; set; }
}

