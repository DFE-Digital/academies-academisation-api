namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record UpdateSchoolParameter(
		int Id,
		SchoolDetails SchoolDetails,
		ICollection<KeyValuePair<int,LoanDetails>> Loans);
}
