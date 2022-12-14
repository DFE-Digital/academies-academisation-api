namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public record LegacyProjectAddNoteModel(string Subject, string Note, string Author, int ProjectId);

	public record AddNoteRequest(string Subject, string Note, string Author);
}
