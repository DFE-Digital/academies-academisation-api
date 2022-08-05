using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface ISchool
{
	public int Id { get; }
	public ApplicationSchoolDetails Details { get; }
}
