using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class School : ISchool
{
	public School(ApplicationSchoolDetails details)
	{
		Details = details;
	}

	public School(int id, ApplicationSchoolDetails details) : this(details)
	{
		Id = id;
	}

	public int Id { get; internal set; }

	public ApplicationSchoolDetails Details { get; }
}
