using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class AcademyTransferPublicSectorEqualityDutyResponse
	{
		public Likelyhood? HowLikelyImpactProtectedCharacteristics { get; set; }
		public string? WhatWillBeDoneToReduceImpact { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
