using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class School : ISchool
{
	private readonly List<Loan> _loans = new();

	public School(SchoolDetails details)
	{
		Details = details;
	}

	public School(int id, SchoolDetails details) : this(details)
	{
		Id = id;
		_loans = details.Loans;
	}

	public int Id { get; internal set; }

	public SchoolDetails Details { get; set; }

	// leases & loans
	public IReadOnlyCollection<ILoan> Loans => _loans;

	// TODO MR:- leases
}
