using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;

public class School : Entity, ISchool
{
	protected School() { }
	public int Id { get;  set; }

	public SchoolDetails Details { get; set; }

	#region Leases and Loans
	public IEnumerable<Loan> Loans => _loans.AsReadOnly();
	public IEnumerable<Lease> Leases => _leases.AsReadOnly();
	public bool? HasLoans { get; private set; }
	IReadOnlyCollection<ILoan> ISchool.Loans => _loans.AsReadOnly();
	public bool? HasLeases { get; private set; }
	IReadOnlyCollection<ILease> ISchool.Leases => _leases.AsReadOnly();

	private readonly List<Loan> _loans;
	private readonly List<Lease> _leases;

	#endregion

	#region Additional Details

	private string? _trustBenefitDetails { get; set; }
	public string? TrustBenefitDetails => _trustBenefitDetails;
	
	private string? _ofstedInspectionDetails { get; set; }
	public string? OfstedInspectionDetails => _ofstedInspectionDetails;

	private string? _safeguardingDetails { get; set; }
	public string? SafeguardingDetails => _safeguardingDetails;
	
	private string? _localAuthorityReoganisationDetails { get; set; }
	public string? LocalAuthorityReorganisationDetails => _localAuthorityReoganisationDetails;
	
	private string? _localAuthorityClosurePlanDetails { get; set; }
	public string? LocalAuthorityClosurePlanDetails => _localAuthorityClosurePlanDetails;
	
	private string? _dioceseName { get; set; }
	public string? DioceseName => _dioceseName;

	private string? _dioceseFolderIdentifier;
	public string? DioceseFolderIdentifier => _dioceseFolderIdentifier;
	
	private bool? _partOfFederation { get; set; }
	public bool? PartOfFederation => _partOfFederation;
	
	private string? _foundationTrustOrBodyName { get; set; }
	public string? FoundationTrustOrBodyName => _foundationTrustOrBodyName;

	private string? _foundationConsentFolderIdentifier;
	public string? FoundationConsentFolderIdentifier => _foundationConsentFolderIdentifier;

	private DateTimeOffset? _exemptionEndDate { get; set; }
	public DateTimeOffset? ExemptionEndDate => _exemptionEndDate;
	
	private string? _mainFeederSchools { get; set; }
	public string? MainFeederSchools => _mainFeederSchools;
	
	private string? _resolutionConsentFolderIdentifier;
	public string? ResolutionConsentFolderIdentifier => _resolutionConsentFolderIdentifier;

	private SchoolEqualitiesProtectedCharacteristics? _protectedCharacteristics { get; set; }
	public SchoolEqualitiesProtectedCharacteristics? ProtectedCharacteristics => _protectedCharacteristics;
	
	private string? _furtherInformation { get; set; }
	public string? FurtherInformation => _furtherInformation;


	#endregion

	
	
	
	private School(SchoolDetails details)
	{
		Details = details;
		_loans = new();
		_leases = new();
	}

	public School(int id, 	
		string? trustBenefitDetails, 
		string? ofstedInspectionDetails, 
		string? safeguardingDetails, 
		string? localAuthorityReorganisationDetails,
		string? localAuthorityClosurePlanDetails,
		string? dioceseName,
		string? dioceseFolderIdentifier,
		bool? partOfFederation,
		string? foundationTrustOrBodyName,
		string? foundationConsentFolderIdentifier,
		DateTimeOffset? exemptionEndDate,
		string? mainFeederSchools,
		string? resolutionConsentFolderIdentifier,
		SchoolEqualitiesProtectedCharacteristics? protectedCharacteristics,
		string? furtherInformation, 
		SchoolDetails details, 
		IEnumerable<Loan> loans, 
		IEnumerable<Lease> leases,
		bool? hasLoans, bool? hasLeases) : this(details)
	{
		Id = id;
		_trustBenefitDetails = trustBenefitDetails;
		_ofstedInspectionDetails = ofstedInspectionDetails;
		_safeguardingDetails = safeguardingDetails;
		_localAuthorityReoganisationDetails = localAuthorityReorganisationDetails;
		_localAuthorityClosurePlanDetails = localAuthorityClosurePlanDetails;
		_dioceseName = dioceseName;
		_dioceseFolderIdentifier = dioceseFolderIdentifier;
		_partOfFederation = partOfFederation;
		_foundationTrustOrBodyName = foundationTrustOrBodyName;
		_foundationConsentFolderIdentifier = foundationConsentFolderIdentifier;
		_exemptionEndDate = exemptionEndDate;
		_mainFeederSchools = mainFeederSchools;
		_resolutionConsentFolderIdentifier = resolutionConsentFolderIdentifier;
		_protectedCharacteristics = protectedCharacteristics;
		_furtherInformation = furtherInformation;
		_loans = loans.ToList();
		_leases = leases.ToList();
		HasLoans = hasLoans;
		HasLeases = hasLeases;
	}
	public void AddLoan(decimal amount, string purpose, string provider, decimal interestRate, string schedule)
	{
		HasLoans = true;
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
		HasLoans = _loans.Any();
	}
	
	public void AddLease(string leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
	{
		_leases.Add(new Lease(0, leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets, responsibleForAssets));
		HasLeases = true;
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
		HasLeases = _leases.Any();
	}

	public void SetAdditionalDetails(
		string trustBenefitDetails, 
		string? ofstedInspectionDetails, 
		string? safeguardingDetails, 
		string? localAuthorityReorganisationDetails,
		string? localAuthorityClosurePlanDetails,
		string? dioceseName,
		string dioceseFolderIdentifier,
		bool partOfFederation,
		string? foundationTrustOrBodyName,
		string foundationConsentFolderIdentifier,
		DateTimeOffset? exemptionEndDate,
		string mainFeederSchools,
		string resolutionConsentFolderIdentifier,
		SchoolEqualitiesProtectedCharacteristics? protectedCharacteristics,
		string? furtherInformation)
	{
		_trustBenefitDetails = trustBenefitDetails;
		_ofstedInspectionDetails = ofstedInspectionDetails;
		_safeguardingDetails = safeguardingDetails;
		_localAuthorityReoganisationDetails = localAuthorityReorganisationDetails;
		_localAuthorityClosurePlanDetails = localAuthorityClosurePlanDetails;
		_dioceseName = dioceseName;
		_dioceseFolderIdentifier = dioceseFolderIdentifier;
		_partOfFederation = partOfFederation;
		_foundationTrustOrBodyName = foundationTrustOrBodyName;
		_foundationConsentFolderIdentifier = foundationConsentFolderIdentifier;
		_exemptionEndDate = exemptionEndDate;
		_mainFeederSchools = mainFeederSchools;
		_resolutionConsentFolderIdentifier = resolutionConsentFolderIdentifier;
		_protectedCharacteristics = protectedCharacteristics;
		_furtherInformation = furtherInformation;
	}
}
