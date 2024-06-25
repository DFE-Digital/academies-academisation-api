using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class SchoolImprovementPlanServiceModel
	{
		public SchoolImprovementPlanServiceModel(int id,
			int projectId,
			List<SchoolImprovementPlanArranger> arrangedBy,
			string? arrangedByOther,
			string providedBy,
			DateTime startDate,
			SchoolImprovementPlanExpectedEndDate expectedEndDate,
			DateTime? expectedEndDateOther,
			SchoolImprovementPlanConfidenceLevel confidenceLevel,
			string? planComments,
			DateTime createdDate)
		{
			Id = id;
			ProjectId = projectId;
			ArrangedBy = arrangedBy;
			ArrangedByOther = arrangedByOther;
			ProvidedBy = providedBy;
			StartDate = startDate;
			ExpectedEndDate = expectedEndDate;
			ExpectedEndDateOther = expectedEndDateOther;
			ConfidenceLevel = confidenceLevel;
			PlanComments = planComments;
			CreatedDate = createdDate;
		}

		public int Id { get; set;  }
		public int ProjectId { get; set; }

		public List<SchoolImprovementPlanArranger> ArrangedBy { get; set; }
		public string? ArrangedByOther { get; set; }
		public string ProvidedBy { get; set; }
		public DateTime StartDate { get; private set; }
		public SchoolImprovementPlanExpectedEndDate ExpectedEndDate { get; set; }
		public DateTime? ExpectedEndDateOther { get; set; }
		public SchoolImprovementPlanConfidenceLevel ConfidenceLevel { get; set; }
		public string? PlanComments { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
