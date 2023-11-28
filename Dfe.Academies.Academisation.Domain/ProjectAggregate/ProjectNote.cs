using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public class ProjectNote : IProjectNote
	{
		public ProjectNote(string? subject,
						   string? note,
						   string? author,
						   DateTime? date, int projectId)
		{
			Subject = subject;
			Note = note;
			Author = author;
			Date = date;
			ProjectId = projectId;
		}
		public int ProjectId { get; private set; }
		public int Id { get; private set; }
		public string? Subject { get; private set; }
		public string? Note { get; private set; }
		public string? Author { get; private set; }
		public DateTime? Date { get; private set; }
	}
}
