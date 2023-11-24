using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;

namespace Dfe.Academies.Academisation.Service.Extensions
{
	public static class LegacyProjectAddNoteModelExtensions
	{
		public static ProjectNote ToProjectNote(this ConversionProjectAddNoteCommand model)
		{
			return new ProjectNote(model.Subject, model.Note, model.Author, model.Date, model.ProjectId);
		}
	}
}
