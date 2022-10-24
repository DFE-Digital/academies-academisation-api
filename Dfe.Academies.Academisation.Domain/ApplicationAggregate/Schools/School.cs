using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;

public class School : ISchool
{
	private readonly List<Loan> _loans;
	private readonly List<Lease> _leases;
	private School(SchoolDetails details)
	{
		Details = details;
		_loans = new();
		_leases = new();
	}

	public School(int id, SchoolDetails details, IEnumerable<Loan> loans, IEnumerable<Lease> leases) : this(details)
	{
		Id = id;
		_loans = loans.ToList();
		_leases = leases.ToList();
	}

	public int Id { get;  set; }

	public SchoolDetails Details { get; set; }

	// leases & loans
	public IReadOnlyCollection<ILoan> Loans => _loans.AsReadOnly();
	public IReadOnlyCollection<ILease> Leases => _leases.AsReadOnly();

	public void AddLoan(decimal amount, string purpose, string provider, decimal interestRate, string schedule)
	{
		_loans.Add(Loan.Create(amount, purpose, provider, interestRate, schedule));
	}

	public void UpdateLoan(int id, decimal amount, string purpose, string provider, decimal interestRate,
		string schedule)
	{
		var loan = _loans.FirstOrDefault(x => x.Id == id);
		loan?.Update(amount, purpose, provider, interestRate, schedule);
	}

	public void DeleteLoan(int id)
	{
		var loan = _loans.FirstOrDefault(x => x.Id == id);
		if (loan == null) return;
		_loans.Remove(loan);
	}
	
	public void AddLease(string leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
	{
		_leases.Add(new Lease(0, leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets, responsibleForAssets));
	}
	
	public void UpdateLease(int id, string leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
	{
		var lease = _leases.FirstOrDefault(x => x.Id == id);
		lease?.Update(leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets, responsibleForAssets);
	}
	
	public void DeleteLease(int id)
	{
		var lease = _leases.FirstOrDefault(x => x.Id == id);
		if (lease == null) return;
		_leases.Remove(lease);
	}
}
