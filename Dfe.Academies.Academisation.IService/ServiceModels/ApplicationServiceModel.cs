using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels;

public class ApplicationServiceModel
{
	public int ApplicationId { get; set; } 
	public ApplicationType ApplicationType { get; set; }
	public ApplicationStatus ApplicationStatus { get; set; }

	public IReadOnlyCollection<ApplicationContributorServiceModel> Contributors { get; set; }

	public IReadOnlyCollection<ApplyingSchoolServiceModel> Schools { get; set; }
}
