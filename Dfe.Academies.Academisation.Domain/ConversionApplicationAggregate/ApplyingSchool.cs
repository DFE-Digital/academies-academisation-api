using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ApplyingSchool : IApplyingSchool
{
	public ApplyingSchool(ApplyingSchoolDetails details)
	{
		Details = details;
	}

	public ApplyingSchool(int id, ApplyingSchoolDetails details) : this(details)
	{
		Id = id;
	}

	public int Id { get; internal set; }

	public ApplyingSchoolDetails Details { get; }
}
