using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface ISchool
{
	public int Id { get; }
	Guid EntityId { get; }
	public SchoolDetails Details { get; }
	public string? TrustBenefitDetails { get; } 
	public string? OfstedInspectionDetails{ get; }
	public bool? Safeguarding{ get; }
	public string? LocalAuthorityReorganisationDetails{ get; }
	public string? LocalAuthorityClosurePlanDetails{ get; }
	public string? DioceseName{ get; }
	public string? DioceseFolderIdentifier{ get; }
	public bool? PartOfFederation{ get; }
	public string? FoundationTrustOrBodyName{ get; }
	public string? FoundationConsentFolderIdentifier{ get; }
	public DateTimeOffset? ExemptionEndDate{ get; }
	public string? MainFeederSchools{ get; }
	public string? ResolutionConsentFolderIdentifier{ get; }
	public SchoolEqualitiesProtectedCharacteristics? ProtectedCharacteristics{ get; }
	public string? FurtherInformation{ get; }
	public bool? HasLoans { get; }
	public IReadOnlyCollection<ILoan> Loans { get; }

	public bool? HasLeases { get; }
	public IReadOnlyCollection<ILease> Leases { get; }

	void Update(UpdateSchoolParameter schoolUpdate);

}
