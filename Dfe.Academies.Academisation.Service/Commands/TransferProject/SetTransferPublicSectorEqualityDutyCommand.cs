using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferPublicSectorEqualityDutyCommand : SetTransferProjectCommand
	{
		public Likelyhood HowLikelyImpactProtectedCharacteristics { get; set; }
		public string WhatWillBeDoneToReduceImpact { get; set; }
		public bool IsCompleted { get; set; }
	}
}
