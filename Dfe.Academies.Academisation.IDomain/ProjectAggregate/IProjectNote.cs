using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate
{
	public interface IProjectNote
	{

		public int ProjectId { get; set; }
		public int Id { get; set; }
		public string? Subject { get; init; }
		public string? Note { get; init; }
		public string? Author { get; init; }
		public DateTime? Date { get; init; }
	}
}
