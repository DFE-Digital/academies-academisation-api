﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class TransferringAcademiesResponse
	{
		public string OutgoingAcademyUkprn { get; set; }
		public string? IncomingTrustUkprn { get; set; }
		public string IncomingTrustName { get; set; }


		public string PupilNumbersAdditionalInformation { get; set; }
		public string LatestOfstedReportAdditionalInformation { get; set; }
		public string KeyStage2PerformanceAdditionalInformation { get; set; }
		public string KeyStage4PerformanceAdditionalInformation { get; set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }
	}
}
