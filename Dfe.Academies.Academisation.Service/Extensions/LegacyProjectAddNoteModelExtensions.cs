using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Extensions
{
	public static class LegacyProjectAddNoteModelExtensions
	{
		public static ProjectNote ToProjectNote(this LegacyProjectAddNoteModel model)
		{
			return new ProjectNote(model.Subject, model.Note, model.Author, model.Date);
		}
	}
}
