namespace Dfe.Academies.Academisation.IDomain.TransferProjectAggregate
{
	public interface ITransferringAcademy
	{
		int Id { get; }
		string? IncomingTrustUkprn { get; }
		string? IncomingTrustName { get; }
		string? KeyStage2PerformanceAdditionalInformation { get; }
		string? KeyStage4PerformanceAdditionalInformation { get; }
		string? KeyStage5PerformanceAdditionalInformation { get; }
		string? LatestOfstedReportAdditionalInformation { get; }
		string OutgoingAcademyUkprn { get; }
		string? PupilNumbersAdditionalInformation { get; }
		int TransferProjectId { get; }
		string? PFIScheme { get; }
		string? PFISchemeDetails { get; }

		void SetSchoolAdditionalData(string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation, string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation, string keyStage5PerformanceAdditionalInformation);

		void SetIncomingTrustName(string incomingTrustName, string? incomingTrustUKPRN);

		void SetGeneralInformation(string pfiScheme, string pfiSchemeDetails);
	}
}
