using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface ISchool
{
	public int Id { get; }
	public SchoolDetails Details { get; }
	public IReadOnlyCollection<ILoan> Loans { get; }

	// TODO:- Leases
}
