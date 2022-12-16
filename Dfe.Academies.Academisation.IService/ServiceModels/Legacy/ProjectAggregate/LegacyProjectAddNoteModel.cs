namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public record LegacyProjectAddNoteModel(string Subject, string Note, string Author, DateTime Date, int ProjectId);
}
