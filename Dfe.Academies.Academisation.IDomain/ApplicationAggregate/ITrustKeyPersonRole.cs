using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ITrustKeyPersonRole
	{
		int Id { get;  }
		KeyPersonRole Role { get; }
		string TimeInRole { get; }

		void Update(KeyPersonRole role, string timeInRole);
	}
}
