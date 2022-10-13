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
		private FormTrust(int id, FormTrustDetails? trustDetails = null)
		{
			this.Id = id;
			this.TrustDetails = trustDetails!;
		}

		public FormTrustDetails TrustDetails { get; private set; }
		public int Id { get; }

		public static IFormTrust Create(FormTrustDetails trustDetails) {

			return new FormTrust(0, trustDetails);
		}

		public void Update(FormTrustDetails formTrustDetails)
		{
			TrustDetails = formTrustDetails;
		}
	}
}
