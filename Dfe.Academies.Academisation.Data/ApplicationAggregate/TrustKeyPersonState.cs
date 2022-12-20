//using System.ComponentModel.DataAnnotations.Schema;
//using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

//namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
//{
//	[Table(name: "ApplicationFormTrustKeyPerson")]
//	public class TrustKeyPersonState : BaseEntity
//	{
//		public string Name { get; set; }

//		public DateTime DateOfBirth { get; set; }
//		public string Biography { get; set; }

//		// MR:- below mods for Dynamics -> SQL server A2B external app conversion
//		public Guid DynamicsKeyPersonId { get; set; }


//		[ForeignKey("ApplicationFormTrustKeyPersonRoleId")]
//		public HashSet<TrustKeyPersonRoleState> Roles { get; set; } = new();
//	}
//}
