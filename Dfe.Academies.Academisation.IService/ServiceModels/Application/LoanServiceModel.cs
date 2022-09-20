namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record LoanServiceModel(
		int LoanId,
		//// MR:- below props from A2C-SIP - SchoolLoan object
		decimal? Amount,
		string Purpose,
		string Provider,
		decimal? InterestRate,
		string? Schedule
	);
}
