using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;

namespace Dfe.Academies.Academisation.WebApi.Extensions
{
	public static class AddNoteRequestExtensions
	{
		public static ConversionProjectAddNoteCommand ToAddNoteModel(this AddNoteRequest request, int id)
		{
			return new ConversionProjectAddNoteCommand(
				Subject: request.Subject,
				Note: request.Note,
				Author: request.Author,
				Date: request.Date,
				ProjectId: id);
		}
	}
}
