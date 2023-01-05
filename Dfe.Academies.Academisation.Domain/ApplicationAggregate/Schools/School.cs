using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;

public class School : DynamicsSchoolEntity, ISchool
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

	public string? TrustBenefitDetails { get; private set; }
	
	public string? OfstedInspectionDetails { get; private set; }

	public string? SafeguardingDetails { get; private set; }

	public string? LocalAuthorityReorganisationDetails { get; private set; }

	public string? LocalAuthorityClosurePlanDetails { get; private set; }

	public string? DioceseName { get; set; }

	public string? DioceseFolderIdentifier { get; private set; }

	public bool? PartOfFederation { get; private set; }

	public string? FoundationTrustOrBodyName { get; private set; }

	public string? FoundationConsentFolderIdentifier { get; private set; }

	public DateTimeOffset? ExemptionEndDate { get; private set; }

	public string? MainFeederSchools { get; private set; }

	public string? ResolutionConsentFolderIdentifier { get; private set; }

	public SchoolEqualitiesProtectedCharacteristics? ProtectedCharacteristics { get; private set; }

	public string? FurtherInformation { get; private set; }


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
		TrustBenefitDetails = trustBenefitDetails;
		OfstedInspectionDetails = ofstedInspectionDetails;
		SafeguardingDetails = safeguardingDetails;
		LocalAuthorityReorganisationDetails = localAuthorityReorganisationDetails;
		LocalAuthorityClosurePlanDetails = localAuthorityClosurePlanDetails;
		DioceseName = dioceseName;
		DioceseFolderIdentifier = dioceseFolderIdentifier;
		PartOfFederation = partOfFederation;
		FoundationTrustOrBodyName = foundationTrustOrBodyName;
		FoundationConsentFolderIdentifier = foundationConsentFolderIdentifier;
		ExemptionEndDate = exemptionEndDate;
		MainFeederSchools = mainFeederSchools;
		ResolutionConsentFolderIdentifier = resolutionConsentFolderIdentifier;
		ProtectedCharacteristics = protectedCharacteristics;
		FurtherInformation = furtherInformation;
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
		TrustBenefitDetails = trustBenefitDetails;
		OfstedInspectionDetails = ofstedInspectionDetails;
		SafeguardingDetails = safeguardingDetails;
		LocalAuthorityReorganisationDetails = localAuthorityReorganisationDetails;
		LocalAuthorityClosurePlanDetails = localAuthorityClosurePlanDetails;
		DioceseName = dioceseName;
		DioceseFolderIdentifier = dioceseFolderIdentifier;
		PartOfFederation = partOfFederation;
		FoundationTrustOrBodyName = foundationTrustOrBodyName;
		FoundationConsentFolderIdentifier = foundationConsentFolderIdentifier;
		ExemptionEndDate = exemptionEndDate;
		MainFeederSchools = mainFeederSchools;
		ResolutionConsentFolderIdentifier = resolutionConsentFolderIdentifier;
		ProtectedCharacteristics = protectedCharacteristics;
		FurtherInformation = furtherInformation;
	}

	public void Update(UpdateSchoolParameter schoolUpdate)
	{
		//school.Loans.Select(l => new Loan(l.Key, l.Value.Amount!.Value, l.Value.Purpose!, l.Value.Provider!, l.Value.InterestRate!.Value, l.Value.Schedule!)),
		//school.Leases.Select(l => new Lease(l.Key, l.Value.leaseTerm, l.Value.repaymentAmount, l.Value.interestRate, l.Value.paymentsToDate, l.Value.purpose, l.Value.valueOfAssets, l.Value.responsibleForAssets)),

		// this update is to handle any legacy updates that are updating the entore application using the put
		TrustBenefitDetails = schoolUpdate.TrustBenefitDetails;
		OfstedInspectionDetails = schoolUpdate.OfstedInspectionDetails;
		SafeguardingDetails = schoolUpdate.SafeguardingDetails;
		LocalAuthorityReorganisationDetails = schoolUpdate.LocalAuthorityReorganisationDetails;
		LocalAuthorityClosurePlanDetails = schoolUpdate.LocalAuthorityClosurePlanDetails;
		DioceseName = schoolUpdate.DioceseName;
		DioceseFolderIdentifier = schoolUpdate.DioceseFolderIdentifier;
		PartOfFederation = schoolUpdate.PartOfFederation;
		FoundationTrustOrBodyName = schoolUpdate.FoundationTrustOrBodyName;
		FoundationConsentFolderIdentifier = schoolUpdate.FoundationConsentFolderIdentifier;
		ExemptionEndDate = schoolUpdate.ExemptionEndDate;
		MainFeederSchools = schoolUpdate.MainFeederSchools;
		ResolutionConsentFolderIdentifier = schoolUpdate.ResolutionConsentFolderIdentifier;
		ProtectedCharacteristics = schoolUpdate.ProtectedCharacteristics;
		FurtherInformation = schoolUpdate.FurtherInformation;
		Details = schoolUpdate.SchoolDetails;
		//_loans = loans.ToList();
		//_leases = leases.ToList();
		HasLoans = schoolUpdate.HasLoans;
		HasLeases = schoolUpdate.HasLeases;
	}
}
