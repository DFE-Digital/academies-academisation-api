using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate
{
	public class ApplicationUpdateDataCommand : IApplicationUpdateDataCommand
	{
		private readonly AcademisationContext _context;

		public ApplicationUpdateDataCommand(AcademisationContext context)
		{
			_context = context;
		}

		public async Task Execute(IConversionApplication conversionApplication)
		{
			ConversionApplicationState state = ConversionApplicationState.MapFromDomain(conversionApplication);
			
			_context.ConversionApplications.Update(state);
			await _context.SaveChangesAsync();
		}
	}
}
