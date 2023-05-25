using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.RequestModels;

/// <summary>
/// Represents a request model sent to the API.
/// Note that this type is replicated in the Web API Contracts project
/// </summary>
public record ApplicationUpdateRequestModel(
	int ApplicationId,
	ApplicationType ApplicationType,
	ApplicationStatus ApplicationStatus,
	IReadOnlyCollection<ApplicationContributorServiceModel> Contributors,
	IReadOnlyCollection<ApplicationSchoolServiceModel> Schools);
