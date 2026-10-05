using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Seed;

public static class SeedProject
{
	private const int BatchSize = 40;
	private static readonly Random random = new();

	public static async Task CreateProject(AcademisationContext academisationContext, int projectsPerType)
	{
		if (projectsPerType <= 0)
		{
			Console.WriteLine("Please enter a positive number to seed projects.");
			return;
		}

		var conversionProjects = new List<Project>();
		var transferProjects = new List<TransferProject>();
		var significantChangeProjects = new List<SignificantChangeProject>();
		var projectNotes = new List<ProjectNote>();

		for (int i = 0; i < projectsPerType; i++)
		{
			conversionProjects.Add(new Project(default, NewConversionProjectDetails(i)));
			transferProjects.Add(NewTransferProject());
			significantChangeProjects.Add(NewSignificantChangeProject(i));

			if ((i + 1) % BatchSize != 0 && i != projectsPerType - 1)
			{
				continue;
			}

			await using var dbContextTransaction = await academisationContext.Database.BeginTransactionAsync();
			try
			{
				academisationContext.Projects.AddRange(conversionProjects);
				academisationContext.TransferProjects.AddRange(transferProjects);
				academisationContext.SignificantChangeProjects.AddRange(significantChangeProjects);
				await academisationContext.SaveChangesAsync();

				foreach (var project in conversionProjects)
				{
					projectNotes.Add(NewProjectNote(project.Id, project.Details.SchoolName));
				}

				academisationContext.ProjectNotes.AddRange(projectNotes);
				await academisationContext.SaveChangesAsync();
				await dbContextTransaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await dbContextTransaction.RollbackAsync();
				Console.WriteLine(ex.ToString());
				throw;
			}

			conversionProjects.Clear();
			transferProjects.Clear();
			significantChangeProjects.Clear();
			projectNotes.Clear();
		}
	}

	private static ProjectDetails NewConversionProjectDetails(int index)
	{
		string route = Pick(ProjectConsts.ConversionRoutes);
		bool isSponsored = route == "Sponsored";
		string region = Pick(ProjectConsts.Regions);
		string localAuthority = Pick(ProjectConsts.LocalAuthorities);
		string schoolName = BuildSchoolName(index);
		DateTime applicationReceivedDate = DateTime.UtcNow.Date.AddDays(-NextInt(45, 540));
		DateTime? htbDate = NextBool(0.5) ? applicationReceivedDate.AddDays(NextInt(20, 190)) : null;
		DateTime? proposedConversionDate = NextBool(0.5) && htbDate.HasValue ? htbDate.Value.AddDays(NextInt(90, 260)) : null;
		bool grantAmountChanged = NextBool(0.35);
		int actualPupilNumbers = NextInt(180, 1650);
		int capacity = actualPupilNumbers + NextInt(20, 420);
		string trustName = Pick(ProjectConsts.TrustNames);
		string trustReference = $"TR{NextInt(10000, 99999)}";

		return new ProjectDetails
		{
			Urn = NextInt(100000, 999999),
			SchoolName = schoolName,
			LocalAuthority = localAuthority,
			ApplicationReferenceNumber = $"A2B_{DateTime.UtcNow:yyyy}_{NextInt(10000, 99999)}",
			ProjectStatus = Pick(ProjectConsts.ConversionStatuses),
			ApplicationReceivedDate = applicationReceivedDate,
			AssignedDate = applicationReceivedDate.AddDays(NextInt(1, 21)),
			HeadTeacherBoardDate = htbDate,
			LocalAuthorityInformationTemplateSentDate = Optional(applicationReceivedDate.AddDays(NextInt(5, 18))),
			LocalAuthorityInformationTemplateReturnedDate = Optional(applicationReceivedDate.AddDays(NextInt(25, 55))),
			ProposedConversionDate = Optional(proposedConversionDate),
			TrustReferenceNumber = trustReference,
			NameOfTrust = trustName,
			AcademyTypeAndRoute = route,
			ConversionSupportGrantType = OptionalRef(isSponsored ? "Sponsored conversion grant" : "Standard conversion support grant"),
			ConversionSupportGrantAmount = Optional(isSponsored ? NextMoney(75000m, 225000m) : NextMoney(0m, 30000m)),
			ConversionSupportGrantAmountChanged = Optional(grantAmountChanged),
			ConversionSupportGrantChangeReason = grantAmountChanged
			? "Grant adjusted to account for estate condition works and integration costs."
			: null,
			Region = region,
			SchoolPhase = OptionalRef(Pick(ProjectConsts.SchoolPhases)),
			SchoolType = isSponsored ? "Academy sponsored conversion" : "Academy converter",
			ActualPupilNumbers = Optional(actualPupilNumbers),
			Capacity = Optional(capacity),
			PublishedAdmissionNumber = OptionalRef(NextInt(30, 360).ToString()),
			PartOfPfiScheme = OptionalRef(NextBool(0.15) ? "Yes" : "No"),
			ViabilityIssues = OptionalRef(actualPupilNumbers >= (int)Math.Floor(capacity * 0.85m) ? "No" : "Yes"),
			FinancialDeficit = OptionalRef(NextBool(0.30) ? "Yes" : "No"),
			SchoolPerformanceAdditionalInformation = OptionalRef("Latest progress data indicates stable outcomes with targeted support in maths and attendance."),
			RationaleForProject = OptionalRef("Conversion supports leadership capacity, curriculum consistency, and trust-wide safeguarding processes."),
			RationaleForTrust = OptionalRef("The trust has strong improvement capacity and proven delivery in schools with similar context."),
			RisksAndIssues = OptionalRef("Main risks relate to staffing transition and ICT migration; mitigations are tracked in the implementation plan."),
			GoverningBodyResolution = Optional(YesNoNotApplicable.Yes),
			Consultation = Optional(NextBool(0.85) ? YesNoNotApplicable.Yes : YesNoNotApplicable.No),
			DiocesanConsent = Optional(YesNoNotApplicable.NotApplicable),
			FoundationConsent = Optional(YesNoNotApplicable.NotApplicable),
			EndOfCurrentFinancialYear = Optional(DateTime.UtcNow.Date.AddMonths(6)),
			EndOfNextFinancialYear = Optional(DateTime.UtcNow.Date.AddMonths(18)),
			RevenueCarryForwardAtEndMarchCurrentYear = Optional(NextMoney(-250000m, 150000m)),
			ProjectedRevenueBalanceAtEndMarchNextYear = Optional(NextMoney(-180000m, 200000m)),
			CapitalCarryForwardAtEndMarchCurrentYear = Optional(NextMoney(-120000m, 250000m)),
			CapitalCarryForwardAtEndMarchNextYear = Optional(NextMoney(-80000m, 300000m)),
			YearOneProjectedPupilNumbers = Optional(actualPupilNumbers + NextInt(-30, 40)),
			YearTwoProjectedPupilNumbers = Optional(actualPupilNumbers + NextInt(-20, 80)),
			YearThreeProjectedPupilNumbers = Optional(actualPupilNumbers + NextInt(-10, 110)),
			PublicEqualityDutyImpact = OptionalRef("Potential impact identified for vulnerable pupil groups during transition."),
			PublicEqualityDutyReduceImpactReason = OptionalRef("Mitigation includes phased onboarding, SEND review meetings, and transport support."),
			PublicEqualityDutySectionComplete = Optional(true),
			SfsoCommissioningOverview = OptionalRef("FHA engagement planned following advisory board to coordinate conversion readiness."),
			SfsoCommissioningRequestedDate = htbDate?.AddDays(-15) ?? null
		};
	}

	private static TransferProject NewTransferProject()
	{
		var transferringAcademies = NewTransferringAcademies();
		string outgoingTrustName = Pick(ProjectConsts.TrustNames);
		var transfer = TransferProject.Create(
			outgoingTrustUkprn: NewUkprn(),
			outgoingTrustName: outgoingTrustName,
			transferringAcademies: transferringAcademies,
			isFormAMat: NextBool(0.15),
			createdOn: DateTime.UtcNow.AddDays(-NextInt(30, 500)));

		transfer.GenerateUrn(NextInt(300000, 999999));
		transfer.GenerateReference();

		string transferType = Pick(ProjectConsts.TransferTypes);
		var advisoryBoardDate = Optional(DateTime.UtcNow.Date.AddDays(NextInt(-60, 180)));
		var targetDateForTransfer = Optional(advisoryBoardDate?.AddDays(NextInt(60, 220)));

		transfer.SetFeatures(
			whoInitiatedTheTransfer: Pick(ProjectConsts.TransferInitiators),
			specificReasonsForTransfer: PickMany(ProjectConsts.TransferReasons, NextInt(1, 3)),
			transferType: transferType,
			isCompleted: true);

		transfer.SetTransferDates(
			advisoryBoardDate: advisoryBoardDate,
			previousAdvisoryBoardDate: advisoryBoardDate?.AddDays(-NextInt(70, 220)),
			expectedDateForTransfer: targetDateForTransfer,
			isCompleted: true);

		transfer.SetRationale(
			projectRationale: OptionalRef("Transfer supports improved educational outcomes and stronger operational resilience."),
			trustSponsorRationale: OptionalRef("Incoming trust demonstrates sustained improvement outcomes and secure governance."),
			isCompleted: Optional(true));

		bool anyRisks = NextBool(0.40);
		transfer.SetBenefitsAndRisks(
			anyRisks: Optional(anyRisks),
			equalitiesImpactAssessmentConsidered: Optional(true),
			selectedBenefits: PickMany(ProjectConsts.TransferBenefits, NextInt(1, 3)),
			otherBenefitValue: OptionalRef("Improved cross-trust attendance and safeguarding support."),
			highProfileShouldBeConsidered: Optional(NextBool(0.25)),
			highProfileFurtherSpecification: OptionalRef("Local press interest expected due to recent inspection publication."),
			complexLandAndBuildingShouldBeConsidered: Optional(NextBool(0.35)),
			complexLandAndBuildingFurtherSpecification: OptionalRef("Site split across multiple titles; legal due diligence in progress."),
			financeAndDebtShouldBeConsidered: Optional(NextBool(0.30)),
			financeAndDebtFurtherSpecification: OptionalRef("Recovery plan agreed with clear monthly monitoring."),
			otherRisksShouldBeConsidered: Optional(anyRisks),
			otherRisksFurtherSpecification: anyRisks
				? "Transition risk managed through staged implementation milestones and contingency staffing."
				: null,
			isCompleted: true);

		transfer.SetLegalRequirements(
			outgoingTrustResolution: "Yes",
			incomingTrustAgreement: "Yes",
			diocesanConsent: Pick(ProjectConsts.ConsentOutcomes),
			isCompleted: Optional(true));

		transfer.SetPublicEqualityDuty(
			publicEqualityDutyImpact: OptionalRef("Potential impact identified for pupils with SEND during transfer period."),
			publicEqualityDutyReduceImpactReason: OptionalRef("Mitigation includes continuity planning, parent engagement, and targeted support transitions."),
			publicEqualityDutySectionComplete: false);

		transfer.SetGeneralInformation(
			recommendation: OptionalRef(Pick(ProjectConsts.TransferRecommendations)),
			author: OptionalRef(Pick(ProjectConsts.CaseWorkers)));

		transfer.SetStatus(Pick(ProjectConsts.TransferStatuses));
		transfer.SetSfsoCommissioning("FHA review to be arranged after board recommendation and before target transfer date.");

		string assignedUser = Pick(ProjectConsts.CaseWorkers);
		transfer.AssignUser(Guid.NewGuid(), $"{ToEmailLocalPart(assignedUser)}@education.gov.uk", assignedUser);

	foreach (string outgoingAcademyUkprn in transferringAcademies.Select(academy => academy.OutgoingAcademyUkprn))
	{
		string pfiScheme = NextBool(0.20) ? "Yes" : "No";

		transfer.SetTransferringAcademyGeneralInformation(
			transferringAcademyUkprn: outgoingAcademyUkprn,
			pfiScheme: pfiScheme,
			pfiSchemeDetails: pfiScheme == "Yes"
				? "PFI arrangement in place with annual estate review."
				: string.Empty,
			distanceFromAcademyToTrustHq: OptionalRef($"{NextInt(4, 75)} miles"),
			distanceFromAcademyToTrustHqDetails: OptionalRef("Travel analysis completed with expected peak journey times."),
			viabilityIssues: OptionalRef(NextBool(0.30) ? "Yes" : "No"),
			financialDeficit: OptionalRef(NextBool(0.25) ? "Yes" : "No"),
			mpNameAndParty: OptionalRef(Pick(ProjectConsts.MembersOfParliament)),
			publishedAdmissionNumber: OptionalRef(NextInt(30, 360).ToString()));

		transfer.SetTransferringAcademiesSchoolData(
			transferringAcademyUkprn: outgoingAcademyUkprn,
			latestOfstedReportAdditionalInformation: OptionalRef("Most recent report highlights improving leadership and safeguarding consistency."),
			pupilNumbersAdditionalInformation: OptionalRef("Pupil roll stable with increased admissions in lower year groups."),
			keyStage2PerformanceAdditionalInformation: OptionalRef("Key stage 2 attainment has improved for reading and maths."),
			keyStage4PerformanceAdditionalInformation: OptionalRef("Progress 8 trend is positive with improved attendance for disadvantaged pupils."),
			keyStage5PerformanceAdditionalInformation: OptionalRef("Post-16 destinations remain stable with increased apprenticeship uptake."));
		}

		return transfer;
	}

	private static SignificantChangeProject NewSignificantChangeProject(int index)
	{
		string schoolName = BuildSchoolName(index + 5000);
		SignificantChangeProjectOptions options = new (
			urn: NextInt(100000, 999999),
			tier: (byte)NextInt(1, 2),
			trustName: Pick(ProjectConsts.TrustNames),
			trustUkprn: NewUkprn(),
			typeOfSignificantChange: Pick(ProjectConsts.SignificantChangeTypes),
			schoolName: schoolName,
			localAuthorityName: Pick(ProjectConsts.LocalAuthorities),
			companiesHouseNumber: NextInt(10000000, 99999999).ToString(),
			regionName: Pick(ProjectConsts.Regions));

		var project = SignificantChangeProject.Create(options, DateTime.UtcNow.AddDays(-NextInt(10, 540)));

		var status = Pick(ProjectConsts.SignificantChangeStatuses);
		project.SetStatus(status);

		string assignedUser = Pick(ProjectConsts.CaseWorkers);
		if (NextBool(0.6)) project.AssignUser(Guid.NewGuid(), $"{ToEmailLocalPart(assignedUser)}@education.gov.uk", assignedUser);
		project.ApplicationId = $"SC-{DateTime.UtcNow:yyyy}-{index + 1:0000}";
		project.ApplicationReference = $"SC_{NextInt(10000, 99999)}";

		var proposedDecisionDate = Optional(DateTime.UtcNow.Date.AddDays(NextInt(14, 180)));
		var proposedChangeDate = Optional(proposedDecisionDate?.AddDays(NextInt(45, 240)));
		project.SetProjectDates(proposedDecisionDate, proposedChangeDate);

		bool trustConsultedStakeholders = NextBool(0.80);
		project.SetStakeholderConsultation(
			Optional(trustConsultedStakeholders),
			trustConsultedStakeholders ? null : OptionalRef("Consultation is being re-run to include newly affected stakeholder groups."));

		bool trustConsultedReligiousBody = NextBool(0.60);
		project.SetReligiousBodyConsultation(
			Optional(trustConsultedReligiousBody),
			trustConsultedReligiousBody ? null : OptionalRef("School has no formal religious character requiring consultation."));

		var consultationDuration = Pick(ProjectConsts.ConsultationDurationAnswers);
		project.SetConsultationDuration(
			Optional(consultationDuration),
			consultationDuration == ConsultationDurationAnswer.No
				? OptionalRef("Initial consultation did not meet minimum period due to exceptional operational pressures.")
				: null);

		bool includeAdmissionVariation = NextBool(0.50);
		project.SetAdmissionVariationConsultation(
			Optional(includeAdmissionVariation),
			includeAdmissionVariation ? null : OptionalRef("No admission number variation is proposed as part of this change."));

		var equalitiesImpact = Optional(Pick(ProjectConsts.EqualitiesImpacts));
		project.SetEqualitiesImpactAssessment(
			equalitiesImpactAssessmentCompleted: NextBool(0.75),
			equalitiesImpactIdentified: equalitiesImpact,
			equalitiesImpactIdentifiedMitigation: equalitiesImpact == EqualitiesImpact.None
				? "No disproportional impacts identified through screening assessment."
				: "Mitigation includes transport support, phased transition, and SEND casework tracking.");

		var stakeholderObjections = Optional(Pick(ProjectConsts.StakeholderObjections));
		project.SetStakeholderObjections(
			stakeholderObjections,
			stakeholderObjections == SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided
				? "Two objections relating to transport distance remain under active review."
				: null);

		if (status != SignificantChangeStatus.PreDecision)
		{
			project.SetReadOnlyDate(DateTime.UtcNow.Date.AddDays(-NextInt(1, 180)));
		}

		return project;
	}

	private static List<TransferringAcademy> NewTransferringAcademies()
	{
		int academyCount = NextInt(1, 3);
		List<TransferringAcademy> list = [];

		for (int i = 0; i < academyCount; i++)
		{
			string region = Pick(ProjectConsts.Regions);
			list.Add(new TransferringAcademy(
				incomingTrustUkprn: NewUkprn(),
				incomingTrustName: Pick(ProjectConsts.TrustNames),
				outgoingAcademyUkprn: $"{NextInt(10000000, 99999999)}",
				region: region,
				localAuthority: Pick(ProjectConsts.LocalAuthorities)));
		}

		return list;
	}

	private static ProjectNote NewProjectNote(int projectId, string? schoolName)
	{
		string author = Pick(ProjectConsts.CaseWorkers);
		return new ProjectNote(
			subject: "Casework update",
			note: $"{schoolName ?? "School"}: next milestone agreed with trust and local authority; due diligence evidence logged.",
			author: author,
			date: DateTime.UtcNow.AddDays(-NextInt(1, 90)),
			projectId: projectId);
	}

	private static string BuildSchoolName(int index)
	{
		string prefix = Pick(ProjectConsts.SchoolNamePrefixes);
		string suffix = Pick(ProjectConsts.SchoolNameSuffixes);
		return $"{prefix} {(index % 7) + 1} {suffix}";
	}

	private static string NewUkprn() => NextInt(10000000, 99999999).ToString();

	private static string ToEmailLocalPart(string name) => name
		.ToLowerInvariant()
		.Replace(" ", ".", StringComparison.Ordinal);

	private static List<string> PickMany(string[] source, int count)
	{
		int size = Math.Clamp(count, 1, source.Length);
		return [..source.OrderBy(_ => random.Next()).Take(size)];
	}

	private static T Pick<T>(IReadOnlyList<T> values) => values[random.Next(values.Count)];

	private static int NextInt(int minInclusive, int maxInclusive) => random.Next(minInclusive, maxInclusive + 1);

	private static bool NextBool(double probabilityTrue) => random.NextDouble() < probabilityTrue;

	private static decimal NextMoney(decimal min, decimal max)
	{
		decimal sample = min + ((decimal)random.NextDouble() * (max - min));
		return Math.Round(sample / 500m, MidpointRounding.AwayFromZero) * 500m;
	}
	private static T? Optional<T>(T? data) where T : struct =>
    	NextBool(0.3) ? data : null;
	private static T? Optional<T>(T data) where T : struct =>
    	NextBool(0.3) ? data : null;

	private static T? OptionalRef<T>(T data) where T : class =>
		NextBool(0.3) ? data : null;
}


