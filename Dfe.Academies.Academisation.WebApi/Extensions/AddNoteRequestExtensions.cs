using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.WebApi.Extensions
{
	public static class AddNoteRequestExtensions
	{
		public static LegacyProjectAddNoteModel ToAddNoteModel(this AddNoteRequest request, int id)
		{
			return new LegacyProjectAddNoteModel(
				Subject: request.Subject,
				Note: request.Note,
				Author: request.Author,
				Date: request.Date,
				ProjectId: id);
		}
	}
}
