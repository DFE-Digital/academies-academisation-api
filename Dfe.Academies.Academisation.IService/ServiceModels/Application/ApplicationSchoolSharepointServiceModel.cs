using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record ApplicationSchoolSharepointServiceModel(
	int Id,
	string ApplicationReference,
	List<SchoolSharepointServiceModel> SchoolSharepointServiceModels);
