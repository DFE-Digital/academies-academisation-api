using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.ServiceModels;

public class ApplicationContributorServiceModel
{
	public int ContributorId { get; set; }
	public string FirstName { get; set; }
	public string LastName  { get; set; }
	public string EmailAddress  { get; set; }
	public ContributorRole Role  { get; set; }
	public string? OtherRoleName  { get; set; }
}
