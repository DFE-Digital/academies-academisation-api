using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.Validations;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate
{
	public class SchoolImprovementPlan : Entity, ISchoolImprovementPlan
	{
		public SchoolImprovementPlan(int projectId,
			List<SchoolImprovementPlanArranger> arrangedBy,
			string? arrangedByOther,
			string providedBy,
			DateTime startDate,
			SchoolImprovementPlanExpectedEndDate expectedEndDate,
			DateTime? expectedEndDateOther,
			SchoolImprovementPlanConfidenceLevel confidenceLevel,
			string? planComments)
		{
			ProjectId = projectId;
			ArrangedBy = arrangedBy;
			ArrangedByOther = arrangedByOther;
			ProvidedBy = providedBy;
			StartDate = startDate;
			ExpectedEndDate = expectedEndDate;
			ExpectedEndDateOther = expectedEndDateOther;
			ConfidenceLevel = confidenceLevel;
			PlanComments = planComments;
		}
		public int ProjectId { get; private set; }

		public List<SchoolImprovementPlanArranger> ArrangedBy { get; private set; }
		public string? ArrangedByOther { get; private set; }
		public string ProvidedBy { get; private set; }
		public DateTime StartDate { get; private set; }
		public SchoolImprovementPlanExpectedEndDate ExpectedEndDate { get; private set; }
		public DateTime? ExpectedEndDateOther { get; private set; }
		public SchoolImprovementPlanConfidenceLevel ConfidenceLevel { get; private set; }
		public string? PlanComments { get; private set; }

		public void Update(List<SchoolImprovementPlanArranger> arrangedBy, string? arrangedByOther, string providedBy, DateTime startDate, SchoolImprovementPlanExpectedEndDate expectedEndDate, DateTime? expectedEndDateOther, SchoolImprovementPlanConfidenceLevel confidenceLevel, string? planComments)
		{
			ArrangedBy = arrangedBy;
			ArrangedByOther = arrangedByOther;
			ProvidedBy = providedBy;
			StartDate = startDate;
			ExpectedEndDate = expectedEndDate;
			ExpectedEndDateOther = expectedEndDateOther;
			ConfidenceLevel = confidenceLevel;
			PlanComments = planComments;
		}


	}
}
