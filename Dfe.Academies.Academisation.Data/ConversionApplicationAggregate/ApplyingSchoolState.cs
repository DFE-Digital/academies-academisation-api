using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ApplyingSchool")]
public class ApplyingSchoolState : BaseEntity
{
	public int Urn { get; set; }

	public static ApplyingSchoolState MapFromDomain(IApplyingSchool applyingSchool)
	{
		return new()
		{
			Id = applyingSchool.Id,
			Urn = applyingSchool.Details.Urn,
		};
	}

	public ApplyingSchoolDetails MapToDomain()
	{
		return new ApplyingSchoolDetails(Urn);
	}
}
