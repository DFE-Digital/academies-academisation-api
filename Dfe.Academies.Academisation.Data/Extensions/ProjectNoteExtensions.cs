using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.Extensions
{
	public static class ProjectNoteExtensions
	{
		public static IEnumerable<ProjectNoteState> ToProjectNoteStates(this IEnumerable<ProjectNote>? notes)
		{
			if (notes is null) return Enumerable.Empty<ProjectNoteState>();

			return notes.Select(note =>
				new ProjectNoteState { Author = note.Author, Note = note.Note, Subject = note.Subject, Date = note.Date }
			);
		}

		public static IEnumerable<ProjectNote> ToProjectNotes(this IEnumerable<ProjectNoteState>? notes)
		{
			if (notes is null) return Enumerable.Empty<ProjectNote>();

			return notes.Select(note =>
				new ProjectNote(note.Subject, note.Note, note.Author, note.Date));
		}
	}
}
