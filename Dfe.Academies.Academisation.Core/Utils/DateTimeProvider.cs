using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
