﻿using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public class ProjectNote : IProjectNote
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
		public int ProjectId { get; set; }
		public int Id { get; set; }
		public string? Subject { get; init; }
		public string? Note { get; init; }
		public string? Author { get; init; }
		public DateTime? Date { get; init; }
	}
}
