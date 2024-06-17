using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate
{
	public interface ISchoolImprovementPlan
	{

		 int ProjectId { get; }
		 int Id { get;  }

		List<SchoolImprovementPlanArranger> ArrangedBy { get;  }
		string ArrangedByOther { get;  }
		string ProvidedBy { get;  }
		DateTime StartDate { get; }
		SchoolImprovementPlanExpectedEndDate ExpectedEndDate { get; }
		DateTime ExpectedEndDateOther { get; }
		SchoolImprovementPlanConfidenceLevel ConfidenceLevel { get; }
		string PlanComments { get; }
	}
}
