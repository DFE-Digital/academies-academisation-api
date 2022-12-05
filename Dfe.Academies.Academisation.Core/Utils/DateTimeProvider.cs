namespace Dfe.Academies.Academisation.Core.Utils
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now
		{
			get { return DateTime.UtcNow; }
		}
	}
}
