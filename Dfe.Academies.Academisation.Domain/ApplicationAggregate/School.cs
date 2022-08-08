using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class School : ISchool
{
	public School(SchoolDetails details)
	{
		Details = details;
	}

	public School(int id, SchoolDetails details) : this(details)
	{
		Id = id;
	}

	public int Id { get; internal set; }

	public SchoolDetails Details { get; }
}
