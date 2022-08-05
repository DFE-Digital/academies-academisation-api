using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

public class ApplicationCreateDataCommand : IApplicationCreateDataCommand
{
	private readonly AcademisationContext _context;
	
	public ApplicationCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}
	
	public async Task Execute(IApplication conversionApplication)
	{
		var conversionApplicationState = ApplicationState.MapFromDomain(conversionApplication);
		
		_context.ConversionApplications.Add(conversionApplicationState);
		await _context.SaveChangesAsync();

		conversionApplication.SetIdsOnCreate(
			conversionApplicationState.Id,
			conversionApplicationState.Contributors.Single().Id);
	}
}
