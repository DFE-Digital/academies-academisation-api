using Dfe.Academies.Academisation.Domain.Core.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeProjectGetByIdResponse : SignificantChangeProjectSearchResponse
{
	public SignificantChangeStakeholderConsultationResponse StakeholderConsultation { get; set; } = new();
	public SignificantChangeConsultationDurationResponse ConsultationDuration { get; set; } = new();
	public EqualitiesImpactAssessmentResponse EqualitiesImpactAssessment { get; init; } = new();
	public SignificantChangeReligiousBodyConsultationResponse ReligiousBodyConsultation { get; set; } = new();
	public SignificantChangeProjectDatesResponse ProjectDates { get; set; } = new();
	public SignificantChangeStakeholderObjectionsResponse StakeholderObjections { get; set; } = new();
	public SignificantChangeAdmissionVariationConsultationResponse AdmissionVariationConsultation { get; set; } = new();
	public SignificantChangeFundingResponse Funding { get; set; } = new();
	public SignificantChangePlanningPermissionResponse PlanningPermission { get; set; } = new();
	public SignificantChangeRecommendationResponse Recommendation { get; set; } = new();
	public SignificantChangeHqtfConsiderationResponse HqtfConsideration { get; set; } = new();
	public SignificantChangeAdmissionVariationResponse AdmissionsVariation { get; set; } = new();
	public Decision? Decision { get; set; }
	public DateTime? DecisionDate { get; set; }

}
