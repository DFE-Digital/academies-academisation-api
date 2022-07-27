using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels
{
	public class ApplicationServiceModel
	{
		public int ApplicationId { get; set; } 
		public ApplicationType ApplicationType { get; set; }

		public IReadOnlyCollection<ApplicationContributorServiceModel> Contributors { get; set; }
	}
}
