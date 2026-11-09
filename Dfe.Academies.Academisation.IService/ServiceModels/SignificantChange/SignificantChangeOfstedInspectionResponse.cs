namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeOfstedInspectionResponse
	{
		public DateTime? InspectionDate { get; set; }
		public bool? MeetsOfstedStandards { get; set; }
		public string? AdditionalInformation { get; set; }
		public string? SupportingEvidence { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
