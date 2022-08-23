using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

[Table(name: "ConversionApplication")]
public class ApplicationState : BaseEntity
{
	public ApplicationType ApplicationType { get; set; }
	public ApplicationStatus ApplicationStatus { get; set; }
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ContributorState> Contributors { get; set; } = new();
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ApplicationSchoolState> Schools { get; set; } = new();

	public static ApplicationState MapFromDomain(IApplication application)
	{
		return new()
		{
			Id = application.ApplicationId,
			CreatedOn = application.CreatedOn,
			LastModifiedOn = application.LastModifiedOn,
			ApplicationStatus = application.ApplicationStatus,
			ApplicationType = application.ApplicationType,
			Contributors = application.Contributors
				.Select(ContributorState.MapFromDomain)
				.ToHashSet(),
			Schools = application.Schools
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

		return new Application(Id, CreatedOn, LastModifiedOn, ApplicationType, ApplicationStatus, contributorsDictionary, schoolsDictionary);
	}
}
