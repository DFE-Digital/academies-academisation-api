using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	public class ApplicationUpdateDataCommand : IApplicationUpdateDataCommand
	{
		private readonly AcademisationContext _context;

		public ApplicationUpdateDataCommand(AcademisationContext context)
		{
			_context = context;
		}

		public async Task Execute(IApplication application)
		{
			ApplicationState state = ApplicationState.MapFromDomain(application);

			_context.Applications.Update(state);
			await _context.SaveChangesAsync();
		}
	}
}
