namespace Dfe.Academies.Academisation.WebApi.Models
{
	public class ValidationErrorModel
	{
		public ErrorModel Errors { get; set; } = new();
		public string Title { get; set; } = string.Empty;
		public int Status { get; set; } = 0;
	}
	public class ErrorModel
	{
		public List<string> DomainValidations { get; set; } = [];

	}
}
