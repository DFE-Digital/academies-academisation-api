using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationJoinTrust")]
	public class JoinTrustState : BaseEntity
	{
		public int UKPrn { get; set; }
		public string TrustName { get; set; } = null!;

		public bool? ChangesToTrust { get; set; }
		public string? ChangesToTrustExplained { get; set; }

		//public static JoinTrustState? MapFromDomain(IJoinTrust joinTrust)
		//{
		//	if (joinTrust == null) return default;

		//	return new()
		//	{
		//		Id = joinTrust.Id,
		//		TrustName = joinTrust.TrustName,
		//		UKPrn = joinTrust.UKPrn
		//	};
		//}

		//public static IJoinTrust MapToDomain(JoinTrustState trust)
		//{
		//	if (trust == null) { return null!; };

		//	return JoinTrust.Create(trust.Id, trust.UKPrn, trust.TrustName, trust.ChangesToTrust, trust.ChangesToTrustExplained);

		//}
	}
}
