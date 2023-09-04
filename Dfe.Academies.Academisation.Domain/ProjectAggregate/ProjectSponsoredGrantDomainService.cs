namespace Dfe.Academies.Academisation.Domain.ProjectAggregate
{
	public static class ProjectSponsoredGrantDomainService
	{
		private const string FastTrackGrantType = "fast track";
		private const string IntermediateGrantType = "intermediate";
		private const string FullGrantType = "full";
		const decimal FastTrackDefault = 70000;
		const decimal IntermediateDefault = 90000;
		const decimal FullDefault = 110000;
		public static decimal? CalculateDefaultSponsoredGrant(string? existingConversionSupportGrantType,
			string? newConversionSupportGrantType, decimal? currentGrantAmount)
		{
			// if it's empty and now becoming a type, set the default
			if (string.IsNullOrEmpty(existingConversionSupportGrantType))
			{
				return DetermineValueFromType(newConversionSupportGrantType, currentGrantAmount);
			}

			// if it's changed type set the new default
			if (existingConversionSupportGrantType != newConversionSupportGrantType)
			{
				return DetermineValueFromType(newConversionSupportGrantType, currentGrantAmount);
			}

			// if it's the same type remain unchanged
			if (existingConversionSupportGrantType == newConversionSupportGrantType)
			{
				return currentGrantAmount;
			}

			return currentGrantAmount;
		}

		private static decimal? DetermineValueFromType(string? grantType, decimal? currentAmount)
		{
			return grantType?.ToLower() switch
			{
				FastTrackGrantType => FastTrackDefault,
				IntermediateGrantType => IntermediateDefault,
				FullGrantType => FullDefault,
				_ => currentAmount
			};
		}
	}
}
