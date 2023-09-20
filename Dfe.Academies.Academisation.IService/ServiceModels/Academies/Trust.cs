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

	/// <summary>
	/// Unique identifier for a trust. urn is null on trust search
	/// </summary>
	public string UkPrn { get; set; }

	/// <summary>
	/// Unique identifier for a school.
	/// </summary>
	public string Urn { get; set; }

	public string GroupName { get; set; }

	public string GroupId { get; set; }

	public string CompaniesHouseNumber { get; set; }

	public Address TrustAddress { get; set; }
}
