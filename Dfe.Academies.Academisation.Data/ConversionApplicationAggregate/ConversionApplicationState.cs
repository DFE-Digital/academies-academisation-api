using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ConversionApplication")]
public class ConversionApplicationState : BaseEntity
{
	public ApplicationType ApplicationType { get; set; }
	public ApplicationStatus ApplicationStatus { get; set; }
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ContributorState> Contributors { get; set; } = new();
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ApplicationSchoolState> Schools { get; set; } = new();

	public static ConversionApplicationState MapFromDomain(IApplication conversionApplication)
	{
		return new()
		{
			Id = conversionApplication.ApplicationId,
			ApplicationStatus = conversionApplication.ApplicationStatus,
			ApplicationType = conversionApplication.ApplicationType,
			Contributors = conversionApplication.Contributors
				.Select(ContributorState.MapFromDomain)
				.ToHashSet(),
			Schools = conversionApplication.Schools
				.Select(ApplicationSchoolState.MapFromDomain)
				.ToHashSet()
		};
	}

	public IApplication MapToDomain()
	{
		var contributorsDictionary = Contributors.ToDictionary(
			c => c.Id,
			c => new ContributorDetails(c.FirstName, c.LastName, c.EmailAddress, c.Role, c.OtherRoleName));

		var schoolsDictionary = Schools.ToDictionary(
			s => s.Id,
			s => s.MapToDomain());

		return new Application(Id, ApplicationType, ApplicationStatus, contributorsDictionary, schoolsDictionary);
	}
}
