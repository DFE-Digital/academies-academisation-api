using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record ApplicationUpdateRequestModel(
	int ApplicationId,
	ApplicationType ApplicationType,
	ApplicationStatus ApplicationStatus,
	IReadOnlyCollection<ApplicationContributorServiceModel> Contributors,
	IReadOnlyCollection<ApplicationSchoolServiceModel> Schools);
