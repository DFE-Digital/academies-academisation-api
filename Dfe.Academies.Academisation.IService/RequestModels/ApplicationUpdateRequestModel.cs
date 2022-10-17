using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public record ApplicationUpdateRequestModel(
	int ApplicationId,
	ApplicationType ApplicationType,
	ApplicationStatus ApplicationStatus,
	IReadOnlyCollection<ApplicationContributorServiceModel> Contributors,
	IReadOnlyCollection<ApplicationSchoolServiceModel> Schools);
