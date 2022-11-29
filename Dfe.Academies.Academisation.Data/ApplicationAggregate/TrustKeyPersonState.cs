using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationFormTrustKeyPerson")]
	public class TrustKeyPersonState : BaseEntity
	{
		public KeyPersonRole Role { get; set; }
		public string TimeInRole { get; set; }
		public string FirstName { get; set; }

		public string Surname { get; set; }

		public string? ContactEmailAddress { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Biography { get; set; }
	}
}
