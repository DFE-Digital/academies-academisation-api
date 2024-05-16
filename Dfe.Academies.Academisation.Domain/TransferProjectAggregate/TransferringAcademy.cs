using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferringAcademy : ITransferringAcademy
	{
		public TransferringAcademy(string? incomingTrustUkprn, string? incomingTrustName, string outgoingAcademyUkprn)
		{
			IncomingTrustUkprn = incomingTrustUkprn;
			IncomingTrustName = incomingTrustName;
			OutgoingAcademyUkprn = outgoingAcademyUkprn;
		}

		public int Id { get; private set; }
		public int TransferProjectId { get; private set; }
		public string OutgoingAcademyUkprn { get; private set; }
		public string? IncomingTrustUkprn { get; private set; }
		public string? IncomingTrustName { get; private set; }

		public string? PupilNumbersAdditionalInformation { get; private set; }
		public string? LatestOfstedReportAdditionalInformation { get; private set; }
		public string? KeyStage2PerformanceAdditionalInformation { get; private set; }
		public string? KeyStage4PerformanceAdditionalInformation { get; private set; }
		public string? KeyStage5PerformanceAdditionalInformation { get; private set; }
		public string? PFIScheme { get; private set; }
		public string? PFISchemeDetails { get; private set; }

		public void SetSchoolAdditionalData(string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation, string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation, string keyStage5PerformanceAdditionalInformation)
		{
			LatestOfstedReportAdditionalInformation = latestOfstedReportAdditionalInformation;
			PupilNumbersAdditionalInformation = pupilNumbersAdditionalInformation;
			KeyStage2PerformanceAdditionalInformation = keyStage2PerformanceAdditionalInformation;
			KeyStage4PerformanceAdditionalInformation = keyStage4PerformanceAdditionalInformation;
			KeyStage5PerformanceAdditionalInformation = keyStage5PerformanceAdditionalInformation;
		}

		public void SetIncomingTrustName(string incomingTrustName, string incomingTrustUKPRN = "")
		{
			IncomingTrustName = incomingTrustName;
			if (!string.IsNullOrEmpty(incomingTrustUKPRN))
			{
				IncomingTrustUkprn = incomingTrustUKPRN;
			}
		}

		public void SetGeneralInformation(string pfiScheme, string pfiSchemeDetails)
		{
			PFIScheme = pfiScheme;
			PFISchemeDetails = pfiSchemeDetails;
		}
	}
}
