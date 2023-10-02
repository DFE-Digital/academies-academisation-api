namespace Dfe.Academies.Academisation.IService.ServiceModels.Academies;

/// <summary>
/// To de-serialize response from trust search && GetTrustByUkPrn :-
/// {{api-host}}/trusts?ukprn=10058464&api-version=V1
/// </summary>
public record Trust
{
	/// <summary>
	/// System.Text de-serialization requires this !!!
	/// </summary>
	public Trust()
	{
		TrustAddress = new Address();
	}

	public string Ukprn { get; set; }
	public string Urn { get; set; }
	public string GroupName { get; set; }
	public string CompaniesHouseNumber { get; set; }
	public string TrustType { get; set; }
	public Address TrustAddress { get; set; }
}
