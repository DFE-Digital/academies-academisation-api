using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels;

/// <summary>
/// Represents a request model sent to the API.
/// Note that this type is replicated in the Web API Contracts project
/// </summary>
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
