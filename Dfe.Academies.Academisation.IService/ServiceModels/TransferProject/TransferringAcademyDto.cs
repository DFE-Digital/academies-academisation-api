namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public record TransferringAcademyDto
	{
		public string OutgoingAcademyUkprn { get; init; }
		public string? IncomingTrustUkprn { get; init; }
		public string? IncomingTrustName { get; init; }
		public string? Region { get; init; }
		public string? LocalAuthority { get; init; }


		public string? PupilNumbersAdditionalInformation { get; init; }
		public string? LatestOfstedReportAdditionalInformation { get; init; }
		public string? KeyStage2PerformanceAdditionalInformation { get; init; }
		public string? KeyStage4PerformanceAdditionalInformation { get; init; }
		public string? KeyStage5PerformanceAdditionalInformation { get; init; }
		public string? PFIScheme { get; init; }
		public string? PFISchemeDetails { get; init; }
	}
}
