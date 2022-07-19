using Dfe.Academies.Academisation.IData;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

public class ApplicationCreateDataCommand : IApplicationCreateDataCommand
{
	private readonly AcademisationContext _context;
	
	public ApplicationCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}
	
	public async Task Execute(IConversionApplication conversionApplication)
	{
		var conversionApplicationState = ConversionApplicationState.MapFromDomain(conversionApplication);
		
		_context.ConversionApplications.Add(conversionApplicationState);
		await _context.SaveChangesAsync();

		conversionApplication.SetIdsOnCreate(
			conversionApplicationState.Id,
			conversionApplicationState.Contributors.Single().Id);
	}
}
