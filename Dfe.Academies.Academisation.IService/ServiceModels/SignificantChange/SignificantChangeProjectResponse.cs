namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeProjectResponse
	{
		public int Id { get; set; }
		public int Urn { get; set; }
		public byte Tier { get; set; }
		public string TrustName { get; set; }
		public string TrustUkprn { get; set; }
		public string TypeOfSignificantChange { get; set; }
		public string Status { get; set; }
	}
}