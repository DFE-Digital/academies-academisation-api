using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Contracts.V4.Establishments;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TransferProjectQueryService : ITransferProjectQueryService
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IAcademiesQueryService _establishmentRepository;
		private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

		public TransferProjectQueryService(
			ITransferProjectRepository transferProjectRepository,
			IAcademiesQueryService establishmentRepository,
			IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
		{
			_transferProjectRepository = transferProjectRepository;
			_establishmentRepository = establishmentRepository;
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
		}

		public async Task<AcademyTransferProjectResponse?> GetByUrn(int id)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(id);

			return AcademyTransferProjectResponseFactory.Create(transferProject);
		}

		public async Task<AcademyTransferProjectResponse?> GetById(int id)
		{
			var transferProject = await _transferProjectRepository.GetById(id);

			return AcademyTransferProjectResponseFactory.Create(transferProject);
		}

		public async Task<PagedResultResponse<AcademyTransferProjectSummaryResponse>> GetTransferProjects(int page, int count, int? urn,
		string title)
		{
			IEnumerable<ITransferProject> transferProjects = FilterByUrn(
			await _transferProjectRepository.GetAllTransferProjects(), urn).ToList();

			//the logic retrieving the trust data goes here
			IEnumerable<AcademyTransferProjectSummaryResponse> projects =
				FilterByIncomingTrust(title, AcademyTransferProjectSummaryResponse(transferProjects));

			// remove any projects without an outgoing trust.
			projects = projects
			.Where(p =>
				!string.IsNullOrEmpty(p.OutgoingTrustUkprn) && !string.IsNullOrEmpty(p.OutgoingTrustName)
				).ToList();

			var recordTotal = projects.Count();

			projects = projects.OrderByDescending(atp => atp.ProjectUrn)
			.Skip((page - 1) * count).Take(count).ToList();

			return await Task.FromResult(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(projects, recordTotal));
		}
		public async Task<PagedDataResponse<AcademyTransferProjectSummaryResponse>?> GetProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count)
		{
			var (projects, totalCount) = await _transferProjectRepository.SearchProjects(states, title, deliveryOfficers, page, count);
			IEnumerable<AcademyTransferProjectSummaryResponse> data = AcademyTransferProjectSummaryResponse(projects);
			var pageResponse = PagingResponseFactory.Create("transfer-projects/projects", page, count, totalCount,
				new Dictionary<string, object?> {
				{"states", states},
				});

			return new PagedDataResponse<AcademyTransferProjectSummaryResponse>(data,
				pageResponse);
		}
		public async Task<PagedResultResponse<ExportedTransferProjectModel>> GetExportedTransferProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count)
		{
			var (projects, totalCount) = await _transferProjectRepository.SearchProjects(states, title, deliveryOfficers, page, count);
			IEnumerable<IConversionAdvisoryBoardDecision> advisoryBoardDecisions;
			try
			{
				advisoryBoardDecisions = await _advisoryBoardDecisionRepository.GetAllAdvisoryBoardDecisionsForTransfers();
			}
			catch (Exception ex)
			{
				var e = ex;
				throw;
			}

			var mappedProjects = await MapExportedTransferProjectModel(projects, advisoryBoardDecisions);

			int recordTotal = mappedProjects.Count();

			mappedProjects = mappedProjects.OrderByDescending(atp => atp.Urn);

			return await Task.FromResult(new PagedResultResponse<ExportedTransferProjectModel>(mappedProjects, recordTotal));
		}

		private static IEnumerable<ITransferProject> FilterByUrn(IEnumerable<ITransferProject> queryable,
		int? urn)
		{
			if (urn.HasValue) queryable = queryable.Where(p => p.Urn == urn);

			return queryable;

		}

		private static IEnumerable<AcademyTransferProjectSummaryResponse> FilterByIncomingTrust(string title,
		IEnumerable<AcademyTransferProjectSummaryResponse> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				queryable = queryable
					.Where(p => p.TransferringAcademies != null && p.TransferringAcademies
						.Exists(r => r != null && r.IncomingTrustName != null && r.IncomingTrustName.ToLower().Contains(title.ToLower())))
					.ToList();
			}
			return queryable;
		}

		private async Task<IEnumerable<ExportedTransferProjectModel>> MapExportedTransferProjectModel(IEnumerable<ITransferProject?> atp, IEnumerable<IConversionAdvisoryBoardDecision> decisions)
		{
			if (atp == null) throw new ArgumentNullException(nameof(atp));

			var projects = new List<ExportedTransferProjectModel>();

			var ukprns = atp.SelectMany(project => project.TransferringAcademies)
							.Select(ta => ta.OutgoingAcademyUkprn)
							.Distinct()
							.ToList();

			var establishments = await _establishmentRepository.GetBulkEstablishmentsByUkprn(ukprns);

			foreach (var transferProject in atp)
			{

				if (transferProject == null)
				{
					continue;
				}

				var _ukprns = transferProject.TransferringAcademies.Select(ta => ta.OutgoingAcademyUkprn);
				var establishmentsForTheseAcademies = establishments.Where(e => _ukprns.Contains(e.Ukprn));
				foreach (var transferringEstablishment in establishmentsForTheseAcademies)
				{
					var decision = decisions.SingleOrDefault(x => x.AdvisoryBoardDecisionDetails.TransferProjectId == transferProject.Id);
					var transferringAcademy = transferProject.TransferringAcademies.Where(s => s.OutgoingAcademyUkprn == transferringEstablishment.Ukprn).Single();
					var project = await MapProject(transferProject, transferringEstablishment, decision, $"{transferringAcademy.PFIScheme} {transferringAcademy.PFISchemeDetails}").ConfigureAwait(false);
					projects.Add(project);
				}
			}

			return projects.AsEnumerable();
		}

		private async Task<ExportedTransferProjectModel> MapProject(ITransferProject? project, EstablishmentDto? school, IConversionAdvisoryBoardDecision? advisoryBoardDecision, string transferringAcademyPfi)
		{
			var transferringAcademies = project.TransferringAcademies;

			var schoolName = school?.Name;
			var schoolType = school.EstablishmentType?.Name;
			var region = school.Gor?.Name;
			var localAuthority = school.LocalAuthorityName;
			var reason = project.SpecificReasonsForTransfer?.JoinNonEmpty(", ");

			return new ExportedTransferProjectModel
			{
				Id = project.Id,
				AssignedUserFullName = string.IsNullOrWhiteSpace(project.AssignedUserEmailAddress) ? null : project.AssignedUserFullName,
				AdvisoryBoardDate = project?.HtbDate ?? null,
				DecisionDate = advisoryBoardDecision?.AdvisoryBoardDecisionDetails?.AdvisoryBoardDecisionDate,
				IncomingTrustName = transferringAcademies.FirstOrDefault()?.IncomingTrustName,
				IncomingTrustUkprn = transferringAcademies.FirstOrDefault()?.IncomingTrustUkprn,
				LocalAuthority = localAuthority,
				OutgoingTrustName = project?.OutgoingTrustName,
				OutgoingTrustUKPRN = project?.OutgoingTrustUkprn,
				ProposedAcademyTransferDate = project.TargetDateForTransfer,
				Region = region,
				SchoolName = schoolName,
				SchoolType = schoolType,
				Status = project.Status,
				TransferReason = reason,
				TransferType = project.WhoInitiatedTheTransfer,
				Urn = school.Urn,
				PFI = transferringAcademyPfi
			};
		}

		public IEnumerable<AcademyTransferProjectSummaryResponse> AcademyTransferProjectSummaryResponse(
 IEnumerable<ITransferProject> atp)
		{
			return atp.Select(x =>
			{
				return new AcademyTransferProjectSummaryResponse
				{
					ProjectUrn = x.Urn.ToString(),
					ProjectReference = x.ProjectReference,
					OutgoingTrustUkprn = x.OutgoingTrustUkprn,
					OutgoingTrustName = x.OutgoingTrustName,
					Status = x.Status,
					AssignedUser = string.IsNullOrWhiteSpace(x.AssignedUserEmailAddress)
				   ? null
				   : new AssignedUserResponse
				   {
					   EmailAddress = x.AssignedUserEmailAddress,
					   FullName = x.AssignedUserFullName,
					   Id = x.AssignedUserId
				   },
					TransferringAcademies = x.TransferringAcademies.Select(ta =>
					{
						return new TransferringAcademyDto
						{
							OutgoingAcademyUkprn = ta.OutgoingAcademyUkprn,
							IncomingTrustUkprn = ta.IncomingTrustUkprn,
							IncomingTrustName = ta.IncomingTrustName,
							PupilNumbersAdditionalInformation = ta.PupilNumbersAdditionalInformation,
							LatestOfstedReportAdditionalInformation = ta.LatestOfstedReportAdditionalInformation,
							KeyStage2PerformanceAdditionalInformation = ta.KeyStage2PerformanceAdditionalInformation,
							KeyStage4PerformanceAdditionalInformation = ta.KeyStage4PerformanceAdditionalInformation,
							KeyStage5PerformanceAdditionalInformation = ta.KeyStage5PerformanceAdditionalInformation,
							PFIScheme = ta.PFIScheme,
							PFISchemeDetails = ta.PFISchemeDetails,
							DistanceFromAcademyToTrustHq = ta.DistanceFromAcademyToTrustHq,
							DistanceFromAcademyToTrustHqDetails = ta.DistanceFromAcademyToTrustHqDetails,
							FinancialDeficit = ta.FinancialDeficit,
							ViabilityIssues = ta.ViabilityIssues,
							MPNameAndParty = ta.MPNameAndParty,
							PublishedAdmissionNumber = ta.PublishedAdmissionNumber
						};
					}).ToList(),
					IsFormAMat = x.IsFormAMat
				};
			});
		}

	}

}
