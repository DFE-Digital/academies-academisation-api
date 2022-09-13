namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record LoanServiceModel(
		int LoanId,
		//// MR:- below props from A2C-SIP - SchoolLoan object
		decimal? Amount,
		string Purpose,
		string Provider,
		decimal? InterestRate,
		string? Schedule,
		// MR:- below are 2 I have thought we perhaps should have?
		DateTime? EndDate,
		short TermMonths
		);
}
