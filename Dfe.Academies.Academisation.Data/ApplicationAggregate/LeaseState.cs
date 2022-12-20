//using System.ComponentModel.DataAnnotations.Schema;
//using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
//using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

//namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
//{
//	[Table(name: "ApplicationSchoolLease")]
//	public class LeaseState : BaseEntity
//	{
//		public string LeaseTerm { get; set; }
//		public decimal RepaymentAmount { get; set;}
//		public decimal InterestRate { get; set; }
//		public decimal PaymentsToDate { get; set; }
//		public string Purpose { get; set; }
//		public string ValueOfAssets { get; set; }
//		public string ResponsibleForAssets { get; set; }

//		// MR:- below mods for Dynamics -> SQL server A2B external app conversion
//		public Guid? DynamicsSchoolLeaseId { get; set; }

//		public static Lease MapFromDomain(ILease lease)
//		{
//			return new Lease(lease.Id, lease.LeaseTerm, lease.RepaymentAmount, lease.InterestRate, lease.PaymentsToDate, lease.Purpose, lease.ValueOfAssets, lease.ResponsibleForAssets);
//		}

//		public Lease MapToDomain()
//		{
//			return new Lease(Id, LeaseTerm, RepaymentAmount, InterestRate, PaymentsToDate, Purpose, ValueOfAssets, ResponsibleForAssets);
//		}
//	}
//}
