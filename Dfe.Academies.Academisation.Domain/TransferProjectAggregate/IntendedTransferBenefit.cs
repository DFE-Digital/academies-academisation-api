using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class IntendedTransferBenefit
	{
		public int Id { get; private set; }
		public int TransferProjectId { get; private set; }
		public string SelectedBenefit { get; private set; }
	}
}
