using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ApplicationSchool : IApplicationSchool
{
	public ApplicationSchool(ApplicationSchoolDetails details)
	{
		Details = details;
	}

	public ApplicationSchool(int id, ApplicationSchoolDetails details) : this(details)
	{
		Id = id;
	}

	public int Id { get; internal set; }

	public ApplicationSchoolDetails Details { get; }
}
