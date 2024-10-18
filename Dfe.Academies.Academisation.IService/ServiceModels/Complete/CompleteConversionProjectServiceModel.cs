using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Complete
{
	public record CompleteConversionProjectServiceModel(
		int urn,
		string? advisory_board_date,
		string? advisory_board_conditions,
		string? provisional_conversion_date,
		bool directive_academy_order,
		string? created_by_email,
		string? created_by_first_name,
		string? created_by_last_name,
		int? prepare_id,
		string? group_id,
		int? incoming_trust_ukprn);

	public record CompleteFormAMatConversionProjectServiceModel(
		int urn,
		string? advisory_board_date,
		string? advisory_board_conditions,
		string? provisional_conversion_date,
		bool directive_academy_order,
		string? created_by_email,
		string? created_by_first_name,
		string? created_by_last_name,
		int? prepare_id,
		string? group_id,
		string new_trust_reference_number,
		string new_trust_name);

	public record CompleteTransferProjectServiceModel(
		int urn,
		string? advisory_board_date,
		string? advisory_board_conditions,
		string? provisional_transfer_date,
		bool? inadequate_ofsted,
		bool? financial_safeguarding_governance_issues,
		bool? outgoing_trust_to_close,
		string? created_by_email,
		string? created_by_first_name,
		string? created_by_last_name,
		int? prepare_id,
		string? group_id,
		int? incoming_trust_ukprn,
		int? outgoing_trust_ukprn);

	public record CompleteFormAMatTransferProjectServiceModel(
		int urn,
		string? advisory_board_date,
		string? advisory_board_conditions,
		string? provisional_transfer_date,
		bool? inadequate_ofsted,
		bool? financial_safeguarding_governance_issues,
		int? outgoing_trust_ukprn,
		bool? outgoing_trust_to_close,
		string? created_by_email,
		string? created_by_first_name,
		string? created_by_last_name,
		int? prepare_id,
		string? group_id,
		string new_trust_reference_number,
		string new_trust_name
	);

}
