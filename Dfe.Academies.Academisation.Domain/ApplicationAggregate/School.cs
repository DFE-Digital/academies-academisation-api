using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class School : ISchool
{
	private readonly List<Loan> _loans = new();

	private School(SchoolDetails details)
	{
		Details = details;
	}

	public School(int id, SchoolDetails details, IEnumerable<Loan> loans) : this(details)
	{
		Id = id;
		_loans = loans.ToList();
	}

	public int Id { get; internal set; }

	public SchoolDetails Details { get; set; }

	// leases & loans
	public IReadOnlyCollection<ILoan> Loans => _loans.AsReadOnly();

	// TODO MR:- leases
}
