using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

[Table(name: "ApplicationSchool")]
public class ApplicationSchoolState : BaseEntity
{
	public int Urn { get; set; }
	public string? ProposedNewSchoolName { get; set; }
	// future pupil numbers
	public int? ProjectedPupilNumbersYear1 { get; set; }
	public int? ProjectedPupilNumbersYear2 { get; set; }
	public int? ProjectedPupilNumbersYear3 { get; set; }
	public string? SchoolCapacityAssumptions { get; set; }
	public int? SchoolCapacityPublishedAdmissionsNumber { get; set; }

	public static ApplicationSchoolState MapFromDomain(ISchool applyingSchool)
	{
		return new()
		{
			Id = applyingSchool.Id,
			Urn = applyingSchool.Details.Urn,
			ProposedNewSchoolName = applyingSchool.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = applyingSchool.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = applyingSchool.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = applyingSchool.Details.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = applyingSchool.Details.SchoolCapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = applyingSchool.Details.SchoolCapacityPublishedAdmissionsNumber
		};
	}

	public SchoolDetails MapToDomain()
	{
		return new SchoolDetails(Urn)
		{
			ProposedNewSchoolName = ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = SchoolCapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = SchoolCapacityPublishedAdmissionsNumber
		};
	}
}
