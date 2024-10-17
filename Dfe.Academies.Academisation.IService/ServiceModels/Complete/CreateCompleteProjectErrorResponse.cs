using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Dfe.Academies.Academisation.IService.ServiceModels.Complete
{
	public class CreateCompleteProjectErrorResponse
	{
		[JsonPropertyName("validation_errors")]
		public ValidationErrors ValidationErrors { get; set; }
		
		public string GetAllErrors()
		{
			var errors = new List<string>();

			if (ValidationErrors != null)
			{
				var allErrorLists = new[]
				{
					ValidationErrors.Urn?.Select(e => $"urn: {e.Error}"),
					ValidationErrors.AdvisoryBoardDate?.Select(e => $"advisory_board_date: {e.Error}"),
					ValidationErrors.AdvisoryBoardConditions?.Select(e => $"advisory_board_conditions: {e.Error}"),
					ValidationErrors.ProvisionalConversionDate?.Select(e => $"provisional_conversion_date: {e.Error}"),
					ValidationErrors.DirectiveAcademyOrder?.Select(e => $"directive_academy_order: {e.Error}"),
					ValidationErrors.CreatedByEmail?.Select(e => $"created_by_email: {e.Error}"),
					ValidationErrors.CreatedByFirstName?.Select(e => $"created_by_first_name: {e.Error}"),
					ValidationErrors.CreatedByLastName?.Select(e => $"created_by_last_name: {e.Error}"),
					ValidationErrors.PrepareId?.Select(e => $"prepare_id: {e.Error}"),
					ValidationErrors.GroupId?.Select(e => $"group_id: {e.Error}"),
					ValidationErrors.IncomingTrustUkprn?.Select(e => $"incoming_trust_ukprn: {e.Error}"),
					ValidationErrors.InadequateOfsted?.Select(e => $"inadequate_ofsted: {e.Error}"),
					ValidationErrors.FinancialSafeguardingGovernanceIssues?.Select(e => $"financial_safeguarding_governance_issues: {e.Error}"),
					ValidationErrors.OutgoingTrustToClose?.Select(e => $"outgoing_trust_to_close: {e.Error}"),
					ValidationErrors.OutgoingTrustUkprn?.Select(e => $"outgoing_trust_to_close: {e.Error}"),
					
				};

				foreach (var errorList in allErrorLists)
				{
					if (errorList != null)
					{
						errors.AddRange(errorList);
					}
				}
			}

			return string.Join(",", errors);
		}
	}
	
	public class ValidationErrors
	{
		[JsonPropertyName("urn")]
		public List<ErrorDetail> Urn { get; set; }

		[JsonPropertyName("advisory_board_date")]
		public List<ErrorDetail> AdvisoryBoardDate { get; set; }
		
		[JsonPropertyName("advisory_board_conditions")]
		
		public List<ErrorDetail> AdvisoryBoardConditions { get; set; }

		[JsonPropertyName("provisional_conversion_date")]
		public List<ErrorDetail> ProvisionalConversionDate { get; set; }
		
		
		[JsonPropertyName("directive_academy_order")]
		public List<ErrorDetail> DirectiveAcademyOrder { get; set; }
		
		[JsonPropertyName("created_by_email")]
		
        public List<ErrorDetail> CreatedByEmail { get; set; }
        
        [JsonPropertyName("created_by_first_name")]
		
        public List<ErrorDetail> CreatedByFirstName { get; set; }
        
        [JsonPropertyName("created_by_last_name")]
		
        public List<ErrorDetail> CreatedByLastName { get; set; }
        
        [JsonPropertyName("prepare_id")]
		
        public List<ErrorDetail> PrepareId { get; set; }
        
        [JsonPropertyName("group_id")]
		
        public List<ErrorDetail> GroupId { get; set; }
        
        [JsonPropertyName("incoming_trust_ukprn")]
		
        public List<ErrorDetail> IncomingTrustUkprn { get; set; }
        
        [JsonPropertyName("inadequate_ofsted")]
		
        public List<ErrorDetail> InadequateOfsted { get; set; }
		
        [JsonPropertyName("financial_safeguarding_governance_issues")]
		
        public List<ErrorDetail> FinancialSafeguardingGovernanceIssues{ get; set; }
		
        [JsonPropertyName("outgoing_trust_to_close")]
        
        public List<ErrorDetail> OutgoingTrustToClose { get; set; }
        
        [JsonPropertyName("outgoing_trust_ukprn")]
		
        public List<ErrorDetail> OutgoingTrustUkprn { get; set; }
	}

	public class ErrorDetail
	{
		[JsonPropertyName("error")]
		public string Error { get; set; }
	}
	
}
