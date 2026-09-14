
namespace Dfe.Academies.Academisation.Core
{
	public static class SfsoCommissioningCalculator
	{
		private const int LeadTimeInDays = 15;
		
		public static DateTime? CalculateRequestedDate(DateTime? proposedDecisionDate, bool mandatoryInformationComplete)
		{
			if (proposedDecisionDate is null || !mandatoryInformationComplete)
			{
				return null;
			}

			DateTime sendDate = proposedDecisionDate.Value.Date.AddDays(-LeadTimeInDays);
			DateTime today = DateTime.Today;
			return sendDate < today ? today : sendDate;
		}
	}
}