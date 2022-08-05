using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ApplicationSchool")]
public class ApplicationSchoolState : BaseEntity
{
	public int Urn { get; set; }

	public static ApplicationSchoolState MapFromDomain(ISchool applyingSchool)
	{
		return new()
		{
			Id = applyingSchool.Id,
			Urn = applyingSchool.Details.Urn,
		};
	}

	public SchoolDetails MapToDomain()
	{
		return new SchoolDetails(Urn);
	}
}
