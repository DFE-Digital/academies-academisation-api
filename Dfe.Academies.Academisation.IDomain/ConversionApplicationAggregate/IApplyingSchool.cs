using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IApplyingSchool
{
	public int Id { get; }
	public ApplyingSchoolDetails Details { get; }
}
