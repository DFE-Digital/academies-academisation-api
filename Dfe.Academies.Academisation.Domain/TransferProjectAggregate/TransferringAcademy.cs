using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferringAcademy : ITransferringAcademy
	{
		public TransferringAcademy(string? incomingTrustUkprn, string? incomingTrustName, string outgoingAcademyUkprn, string? region, string? localAuthority)
		{
			IncomingTrustUkprn = incomingTrustUkprn;
			IncomingTrustName = incomingTrustName;
			OutgoingAcademyUkprn = outgoingAcademyUkprn;
			Region = region;
			LocalAuthority = localAuthority;
		}

		public int Id { get; private set; }
		public int TransferProjectId { get; private set; }
		public string OutgoingAcademyUkprn { get; private set; }
		public string? IncomingTrustUkprn { get; private set; }
		public string? IncomingTrustName { get; private set; }

		public string? Region { get; private set; }
		public string? LocalAuthority { get; private set; }

		public string? PupilNumbersAdditionalInformation { get; private set; }
		public string? LatestOfstedReportAdditionalInformation { get; private set; }
		public string? KeyStage2PerformanceAdditionalInformation { get; private set; }
		public string? KeyStage4PerformanceAdditionalInformation { get; private set; }
		public string? KeyStage5PerformanceAdditionalInformation { get; private set; }
		public string? PFIScheme { get; private set; }
		public string? PFISchemeDetails { get; private set; }
		public string? DistanceFromAcademyToTrustHq { get; private set; }
		public string? DistanceFromAcademyToTrustHqDetails { get; private set; }
		public string? ViabilityIssues { get; set; }
		public string? FinancialDeficit { get; set; }
		public string? MPNameAndParty { get; set; }
		public string? PublishedAdmissionNumber { get; set; }

		public void SetSchoolAdditionalData(string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation, string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation, string keyStage5PerformanceAdditionalInformation)
		{
			LatestOfstedReportAdditionalInformation = latestOfstedReportAdditionalInformation;
			PupilNumbersAdditionalInformation = pupilNumbersAdditionalInformation;
			KeyStage2PerformanceAdditionalInformation = keyStage2PerformanceAdditionalInformation;
			KeyStage4PerformanceAdditionalInformation = keyStage4PerformanceAdditionalInformation;
			KeyStage5PerformanceAdditionalInformation = keyStage5PerformanceAdditionalInformation;
		}

		public void SetIncomingTrust(string incomingTrustName, string? incomingTrustUkprn)
		{
			IncomingTrustName = incomingTrustName;
			if (!string.IsNullOrEmpty(incomingTrustUkprn))
			{
				IncomingTrustUkprn = incomingTrustUkprn;
			}
		}

		public void SetReferenceData(string region, string localAuthority)
		{
			Region = region;
			LocalAuthority = localAuthority;			
		}

		public void SetGeneralInformation(string pfiScheme, string pfiSchemeDetails, string distanceFromAcademyToTrustHq, string distanceFromAcademyToTrustHqDetails, string viabilityIssues, string financialDeficit, string mpNameAndParty, string publishedAdmissionNumber)
		{
			PFIScheme = pfiScheme;
			if (PFIScheme.Contains("No")) pfiSchemeDetails = null;
			PFISchemeDetails = pfiSchemeDetails;
			DistanceFromAcademyToTrustHq = distanceFromAcademyToTrustHq;
			DistanceFromAcademyToTrustHqDetails = distanceFromAcademyToTrustHqDetails;
			ViabilityIssues = viabilityIssues;
			FinancialDeficit = financialDeficit;
			MPNameAndParty = mpNameAndParty;
			PublishedAdmissionNumber = publishedAdmissionNumber;
		}
	}
}
