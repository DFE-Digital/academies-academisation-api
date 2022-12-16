using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	[Table(name: "ProjectNotes")]
	public class ProjectNoteState
	{
		public int Id { get; set; }
		public string? Subject { get; set; }
		public string? Note { get; set; }
		public string? Author { get; set; }
		public DateTime? Date { get; set; }
	}
}
