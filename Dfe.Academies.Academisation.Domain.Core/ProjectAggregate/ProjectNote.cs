namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public class ProjectNote
	{
		public ProjectNote(string? subject,
						   string? note,
						   string? author,
						   DateTime? date)
		{
			Subject = subject;
			Note = note;
			Author = author;
			Date = date;
		}

		public string? Subject { get; init; }
		public string? Note { get; init; }
		public string? Author { get; init; }
		public DateTime? Date { get; init; }
	}
}
