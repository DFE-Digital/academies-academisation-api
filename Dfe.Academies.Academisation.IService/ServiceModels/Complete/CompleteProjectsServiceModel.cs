using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Complete
{
	public record CompleteProjectsServiceModel(
		int urn,
		string? advisory_board_date,
		string? advisory_board_conditions,
		string? provisional_conversion_date,
		bool directive_academy_order,
		string? created_by_email,
		string? created_by_first_name,
		string? created_by_last_name,
		int prepare_id,
		string group_id,
		int? incoming_trust_ukprn);

}
