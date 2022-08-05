using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IApplicationSchool
{
	public int Id { get; }
	public ApplicationSchoolDetails Details { get; }
}
