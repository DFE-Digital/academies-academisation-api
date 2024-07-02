using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans
{
	public enum SchoolImprovementPlanExpectedEndDate
	{
		[Description("To Advisory Board")]
		ToAdvisoryBoard = 1,
		[Description("To Conversion")]
		ToConversion = 2,
		[Description("Unknown")]
		Unknown = 3,
		[Description("Other")]
		Other = 99
	}
}
