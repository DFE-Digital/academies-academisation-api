namespace Dfe.Academies.Academisation.IDomain.TransferProjectAggregate
{
	public interface ITransferringAcademy
	{
		int Id { get; }
		string IncomingTrustUkprn { get; }
		string? KeyStage2PerformanceAdditionalInformation { get; }
		string? KeyStage4PerformanceAdditionalInformation { get; }
		string? KeyStage5PerformanceAdditionalInformation { get; }
		string? LatestOfstedReportAdditionalInformation { get; }
		string OutgoingAcademyUkprn { get; }
		string? PupilNumbersAdditionalInformation { get; }
		int TransferProjectId { get; }
	}
}