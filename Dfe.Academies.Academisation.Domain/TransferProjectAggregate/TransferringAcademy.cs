using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferringAcademy
	{
		public TransferringAcademy(string outgoingAcademyUkprn, string incomingTrustUkprn) {
			IncomingTrustUkprn = incomingTrustUkprn;
			OutgoingAcademyUkprn = outgoingAcademyUkprn;
		}

		public int Id { get; private set; }
		public int TransferProjectId { get; private set; }
		public string OutgoingAcademyUkprn { get; private set; }
		public string IncomingTrustUkprn { get; private set; }

		public string PupilNumbersAdditionalInformation { get; private set; }
		public string LatestOfstedReportAdditionalInformation { get; private set; }
		public string KeyStage2PerformanceAdditionalInformation { get; private set; }
		public string KeyStage4PerformanceAdditionalInformation { get; private set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }

	}
}
