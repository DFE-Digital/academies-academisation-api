using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans
{
	public enum SchoolImprovementPlanConfidenceLevel
	{
		[Description("High")]
		High = 1,
		[Description("Medium")]
		Medium = 2,
		[Description("Low")]
		Low = 3,
		[Description("No Confidence")]
		NoConfidence = 4
	}
}
