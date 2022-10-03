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
	[Table(name: "ApplicationFormTrust")]
	public class FormTrustState : BaseEntity
	{
		public string ProposedTrustName { get; set; } = null!;

		public string? TrustApproverName { get; set; } = null!;

		public static FormTrustState? MapFromDomain(IFormTrust formTrust)
		{
			if (formTrust == null) return default;

			return new()
			{
				Id = formTrust.Id,
				ProposedTrustName = formTrust.ProposedTrustName,
				TrustApproverName = formTrust.TrustDetails?.TrustApproverName
			};
		}

		public static IFormTrust MapToDomain(FormTrustState trust)
		{
			if (trust == null) { return null!; };

			return new FormTrust(trust.Id, trust.ProposedTrustName, new FormTrustDetails(trust.TrustApproverName));

		}
	}
}
