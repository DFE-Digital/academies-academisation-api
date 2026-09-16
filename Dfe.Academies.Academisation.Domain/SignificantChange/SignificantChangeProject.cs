using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{

	public record SignificantChangeProjectOptions(
		int urn, 
		byte tier, 
		string trustName, 
		string trustUkprn, 
		string typeOfSignificantChange, 
		string schoolName, 
		string? localAuthorityName = null, 
		string? companiesHouseNumber = null
	);

	
	public class SignificantChangeProject : Entity, IAggregateRoot
	{
		// Private constructor for EF Core
		private SignificantChangeProject()
		{
		}

		public SignificantChangeProject(SignificantChangeStatus status, SignificantChangeProjectOptions options)
		{
			Status = status;
			Urn = options.urn;
			SchoolName = options.schoolName;
			Tier = options.tier;
			TrustName = options.trustName;
			TrustUkprn = options.trustUkprn;
			TypeOfSignificantChange = options.typeOfSignificantChange;
			LocalAuthorityName = options.localAuthorityName;
			CompaniesHouseNumber = options.companiesHouseNumber;
		}

		public SignificantChangeStatus Status { get; private set; }
		public int Urn { get; private set; }
		public string SchoolName { get; private set; } = string.Empty;
		public byte Tier { get; private set; }
		public Guid? AssignedUserId { get; private set; }
		public string? AssignedUserFullName { get; private set; }
		public string? AssignedUserEmailAddress { get; private set; }
		public string TrustName { get; private set; } = string.Empty;
		public string TrustUkprn { get; private set; } = string.Empty;
		public string TypeOfSignificantChange { get; private set; } = string.Empty;
		public DateTime? ReadOnlyDate { get; private set; }
		public SignificantChangeProjectDetails Details { get; private set; } = new();
		public string? LocalAuthorityName { get; private set; }
		public string? CompaniesHouseNumber { get; private set; }

		public void AssignUser(Guid userId, string userEmail, string userFullName)
		{
			AssignedUserId = userId;
			AssignedUserEmailAddress = userEmail;
			AssignedUserFullName = userFullName;
		}

		public void SetAdmissionVariationConsultation(bool? consultationIncludeAdmissionVariation, string? noAdmissionVariationReason)
		{
			Details.ConsultationIncludeAdmissionVariation = consultationIncludeAdmissionVariation;
			Details.ConsultationNoAdmissionVariationReason = consultationIncludeAdmissionVariation is false
				? noAdmissionVariationReason
				: null;

			if (consultationIncludeAdmissionVariation is false)
			{
				MoveToTierTwoIfApplicable();
			}
		}

		public void SetStakeholderConsultation(bool? trustConsultedStakeholders, string? trustConsultedStakeholdersNotConsultedReason)
		{
			Details.TrustConsultedStakeholders = trustConsultedStakeholders;
			Details.TrustConsultedStakeholdersNotConsultedReason = trustConsultedStakeholders is false
				? trustConsultedStakeholdersNotConsultedReason
				: null;

			if (trustConsultedStakeholders is false)
				MoveToTierTwoIfApplicable();
		}

		public void SetReligiousBodyConsultation(bool? trustConsultedReligiousBody, string? trustConsultedReligiousBodyNotConsultedReason)
		{
			Details.TrustConsultedReligiousBody = trustConsultedReligiousBody;
			Details.TrustConsultedReligiousBodyNotConsultedReason = trustConsultedReligiousBody is false
				? trustConsultedReligiousBodyNotConsultedReason
				: null;

			if (trustConsultedReligiousBody is false)
				MoveToTierTwoIfApplicable();
		}

		public void MoveToTierTwoIfApplicable()
		{
			if (Tier == 1) Tier = 2;
		}

		public static SignificantChangeProject Create(SignificantChangeProjectOptions options, DateTime createdOn)
		{
			return new SignificantChangeProject(SignificantChangeStatus.PreDecision, options) { CreatedOn = createdOn };
		}

		public void SetReadOnlyDate(DateTime readOnlyDate)
		{
			this.ReadOnlyDate = readOnlyDate;
		}

		public void SetEqualitiesImpactAssessment(bool? equalitiesImpactAssessmentCompleted, EqualitiesImpact? equalitiesImpactIdentified, string? equalitiesImpactIdentifiedMitigation)
		{
			Details.EqualitiesImpactAssessmentCompleted = equalitiesImpactAssessmentCompleted;
			Details.EqualitiesImpactIdentified = equalitiesImpactIdentified;
			Details.EqualitiesImpactIdentifiedMitigation = equalitiesImpactIdentifiedMitigation;
      
    }
    
		public void SetProjectDates(DateTime? proposedDecisionDate, DateTime? proposedChangeDate)
		{
			Details.ProposedDecisionDate = proposedDecisionDate;
			Details.ProposedChangeDate = proposedChangeDate;
		}
	}
}
