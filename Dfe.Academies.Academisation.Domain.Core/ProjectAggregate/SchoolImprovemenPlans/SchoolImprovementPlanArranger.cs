using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans
{
	public enum SchoolImprovementPlanArranger
	{
		[Description("Local Authority")]
		LocalAuthority = 1,
		[Description("Regional Director")]
		RegionalDirector = 2,
		[Description("Diocese")]
		Diocese = 3,
		[Description("Other")]
		Other = 99
	}
}
