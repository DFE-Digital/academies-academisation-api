namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public class ProjectNote
	{
		public ProjectNote(string Subject,
						   string Note,
						   string Author)
		{
			this.Subject = Subject;
			this.Note = Note;
			this.Author = Author;
		}

		public string Subject { get; init; }
		public string Note { get; init; }
		public string Author { get; init; }

		public void Deconstruct(out string Subject,
								out string Note,
								out string Author)
		{
			Subject = this.Subject;
			Note = this.Note;
			Author = this.Author;
		}
	}
}
