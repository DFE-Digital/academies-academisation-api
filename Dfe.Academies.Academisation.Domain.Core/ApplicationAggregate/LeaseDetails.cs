﻿namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record LeaseDetails(
		int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate,
		string purpose,
		string valueOfAssets, string responsibleForAssets
	);
}
