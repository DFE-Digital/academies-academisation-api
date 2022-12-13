using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.Extensions
{
	public static class ProjectNoteExtensions
	{
		public static IEnumerable<ProjectNoteServiceModel> ToProjectNoteServiceModels(this IEnumerable<ProjectNote>? notes)
		{
			if (notes is null) return Enumerable.Empty<ProjectNoteServiceModel>();

			return notes.Select(note => new ProjectNoteServiceModel
			{
				Note = note.Note, Author = note.Author, Subject = note.Subject
			});
		}
	}
}