using Dfe.Academies.Academisation.IData;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data;

public class ApplicationCreateDataCommand : IApplicationCreateDataCommand
{
	private readonly AcademisationContext _context;
	
	public ApplicationCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}
	
	public async Task Execute(IConversionApplication conversionApplication)
	{
		// convert 'conversionApplication' to ConversionApplicationState
		ConversionApplicationState conversionApplicationState = new(conversionApplication);
		
		// save to database
		_context.ConversionApplications.Add(conversionApplicationState);
		await _context.SaveChangesAsync();

		// mutate 'conversionApplication' to set Id
		conversionApplication.SetApplicationId(conversionApplicationState.ConversionApplicationId);
	}
}
