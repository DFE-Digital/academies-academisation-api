using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts
{
	public class FormTrust : IFormTrust
	{
		public FormTrust(int id, string? proposedTrustName, FormTrustDetails trustDetails)
		{
			this.Id = id;
			this.ProposedTrustName = proposedTrustName;
			this.TrustDetails = trustDetails;
		}

		public FormTrustDetails TrustDetails { get; }
		public int Id { get; }
		public string? ProposedTrustName { get; }

		public static IFormTrust Create(string? proposedTrustName, FormTrustDetails trustDetails) {

			return new FormTrust(0, proposedTrustName, trustDetails);
		}
	}
}
