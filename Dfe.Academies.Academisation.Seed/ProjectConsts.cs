using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.Seed
{
	public static class ProjectConsts
	{
		public static string[] ConversionStatuses { get; } =
		[
			"Converter Pre-AO (C)", "Approved with Conditions", "Deferred", "Approved", "Declined", "Active",
			"DAO Not Issued", "DAO Revoked", "Withdrawn",
		];
		
		public static string[] ConversionRoutes { get; } =
		[
			"Converter", "Sponsored",
		];
		
		public static string[] Regions { get; } =
		[
			"East Midlands", "East of England", "London", "North East", "North West", "South West", "South East",
			"West Midlands", "Yorkshire and the Humber",
		];
		
		public static string[] LocalAuthorities { get; } =
		[
			"Birmingham", "Bradford", "Cornwall", "Derbyshire", "Essex", "Hackney", "Leeds", "Liverpool",
			"Middlesbrough", "Norfolk", "Nottinghamshire", "Plymouth", "Sandwell", "Sheffield", "Solihull",
			"South Gloucestershire", "Sunderland", "Wandsworth",
		];
		
		public static string[] TrustNames { get; } =
		[
			"Aurora Learning Trust", "Beacon Education Partnership", "Bridgeway Academies Trust",
			"Cedar Grove Learning Trust", "Compass Schools Trust", "Horizon Education Trust",
			"North Star Academies Trust", "Pioneer Futures Trust", "Summit Learning Partnership",
			"Unity Children First Trust",
		];
		
		public static string[] SchoolNamePrefixes { get; } =
		[
			"Ash", "Brook", "Cedar", "Elm", "Harbour", "Kings", "Meadow", "Oak", "Riverside", "St", "West",
		];
		
		public static string[] SchoolNameSuffixes { get; } =
		[
			"Academy", "Community School", "High School", "Primary School", "School", "College",
		];
		
		public static string[] SchoolPhases { get; } =
		[
			"Primary", "Secondary", "All-through", "Special",
		];
		
		public static string[] TransferStatuses { get; } =
		[
			"Withdrawn", "Approved with conditions", "Approved", "Deferred", "Declined",
		];

		public static string[] TransferTypes { get; } =
		[
			"SAT transfer", "MAT transfer", "Trust closure transfer", "Intervention transfer",
		];
		
		public static string[] TransferInitiators { get; } =
		[
			"RSC", "Trust", "Local authority", "Governing body",
		];
		
		public static string[] TransferReasons { get; } =
		[
			"Educational standards", "Trust capacity", "Financial sustainability", "Governance concerns",
			"Place planning", "Pupil outcomes",
		];
		
		public static string[] TransferBenefits { get; } =
		[
			"Improved curriculum breadth", "Stronger school improvement capacity", "Improved financial resilience",
			"Leadership stability", "Improved SEND support", "Better progression pathways",
		];
		
		public static string[] TransferRecommendations { get; } =
		[
			"Proceed to advisory board", "Proceed with conditions", "Hold pending additional evidence",
		];
		
		public static string[] ConsentOutcomes { get; } =
		[
			"Yes", "No", "Not applicable",
		];
		
		public static string[] CaseWorkers { get; } =
		[
			"Alex Johnson", "Cameron Patel", "Charlie Brown", "Jordan Smith", "Morgan Davies", "Riley Green",
			"Samir Khan", "Taylor Campbell",
		];
		
		public static string[] MembersOfParliament { get; } =
		[
			"A. Johnson (Labour)", "C. Davies (Conservative)", "E. Ahmed (Labour)", "H. Smith (Liberal Democrat)",
			"M. Roberts (Conservative)", "R. Khan (Labour)",
		];
		
		public static string[] SignificantChangeTypes { get; } =
		[
			"Change age range", "Expand school capacity", "Establish satellite site", "Add boarding provision",
			"Make prescribed alteration",
		];
		
		public static SignificantChangeStatus[] SignificantChangeStatuses { get; } =
		[
			SignificantChangeStatus.PreDecision,
			SignificantChangeStatus.Approved,
			SignificantChangeStatus.ApprovedWithConditions,
			SignificantChangeStatus.Deferred,
			SignificantChangeStatus.Declined,
		];
		
		public static ConsultationDurationAnswer[] ConsultationDurationAnswers { get; } =
		[
			ConsultationDurationAnswer.Yes,
			ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut,
			ConsultationDurationAnswer.No,
		];
		
		public static EqualitiesImpact[] EqualitiesImpacts { get; } =
		[
			EqualitiesImpact.None,
			EqualitiesImpact.PotentialImpacts,
			EqualitiesImpact.ImpactsIdentified,
		];
		
		public static SignificantChangeStakeholderObjections[] StakeholderObjections { get; } =
		[
			SignificantChangeStakeholderObjections.No,
			SignificantChangeStakeholderObjections.YesAllObjectionsAddressed,
			SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided,
		];
	}
}
