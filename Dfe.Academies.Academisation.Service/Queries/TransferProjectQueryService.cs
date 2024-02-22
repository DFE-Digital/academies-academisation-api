﻿
using AutoMapper;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.Extensions.DependencyInjection;
using Dfe.Academies.Academisation.Service.Extensions;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TransferProjectQueryService : ITransferProjectQueryService
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IMapper _mapper;
		private readonly IAcademiesQueryService _establishmentRepository;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public TransferProjectQueryService(
			ITransferProjectRepository transferProjectRepository,
			IMapper mapper,
			IAcademiesQueryService establishmentRepository,
			IServiceScopeFactory serviceScopeFactory)
		{
			_transferProjectRepository = transferProjectRepository;
			_mapper = mapper;
			_establishmentRepository = establishmentRepository;
			_serviceScopeFactory = serviceScopeFactory;
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

			// remove any projects without an incoming or outgoing trust.
			projects = projects
			.Where(p =>
				!string.IsNullOrEmpty(p.OutgoingTrustUkprn) && !string.IsNullOrEmpty(p.OutgoingTrustName) &&
				!p.TransferringAcademies.Any(ta => string.IsNullOrEmpty(ta.IncomingTrustUkprn) || string.IsNullOrEmpty(ta.IncomingTrustName))).ToList();

			var recordTotal = projects.Count();

			projects = projects.OrderByDescending(atp => atp.ProjectUrn)
			.Skip((page - 1) * count).Take(count).ToList();

			return await Task.FromResult(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(projects, recordTotal));
		}

		public async Task<PagedResultResponse<ExportedTransferProjectModel>> GetExportedTransferProjects(string title)
		{
			IEnumerable<ITransferProject> transferProjects = (await _transferProjectRepository.GetAllTransferProjects()).ToList();

			transferProjects =
				FilterExportedTransferProjectsByIncomingTrust(title, transferProjects);

			// remove any projects without an incoming or outgoing trust.
			transferProjects = transferProjects
			.Where(p =>
				!string.IsNullOrEmpty(p.OutgoingTrustUkprn) && !string.IsNullOrEmpty(p.OutgoingTrustName) &&
				!p.TransferringAcademies.Any(ta => string.IsNullOrEmpty(ta.IncomingTrustUkprn) || string.IsNullOrEmpty(ta.IncomingTrustName))).ToList();

			var projects = await MapExportedTransferProjectModel(transferProjects);

			var recordTotal = projects.Count();

			projects = projects.OrderByDescending(atp => atp.Urn);

			return await Task.FromResult(new PagedResultResponse<ExportedTransferProjectModel>(projects, recordTotal));
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

		private static IEnumerable<ITransferProject> FilterExportedTransferProjectsByIncomingTrust(string title,
		IEnumerable<ITransferProject> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				queryable = queryable
					.Where(p => p.TransferringAcademies != null && p.TransferringAcademies.ToList()
						.Exists(r => r != null && r.IncomingTrustName != null && r.IncomingTrustName.ToLower().Contains(title.ToLower())))
					.ToList();
			}
			return queryable;
		}

		public async Task<IEnumerable<ExportedTransferProjectModel>> MapExportedTransferProjectModel(IEnumerable<ITransferProject> atp)
		{
			if (atp == null) throw new ArgumentNullException(nameof(atp));

			var tasks = atp.Select(MapProject).ToList();

			return await Task.WhenAll(tasks);
		}


		private async Task<ExportedTransferProjectModel> MapProject(ITransferProject project)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var advisoryBoardDecisionRepository = scope.ServiceProvider.GetRequiredService<IAdvisoryBoardDecisionGetDataByProjectIdQuery>();
				var advisoryBoardDecision = await advisoryBoardDecisionRepository.Execute(project.Id);

				var transferringAcademies = project.TransferringAcademies;

				var schools = await Task.WhenAll(
					transferringAcademies.Select(async transferringAcademy =>
					{
						return await _establishmentRepository.GetEstablishmentByUkprn(transferringAcademy?.OutgoingAcademyUkprn);
					})
				);

				var schoolNames = schools.Select(s => s?.EstablishmentName).Distinct().JoinNonEmpty(", ");
				var schoolTypes = schools.Select(s => s?.EstablishmentType?.Name).Distinct().JoinNonEmpty(", ");
				var regions = schools.Select(s => s?.Gor?.Name).Distinct().JoinNonEmpty(", ");
				var localAuthorities = schools.Select(s => s?.LocalAuthorityName).Distinct().JoinNonEmpty(", ");

				return new ExportedTransferProjectModel
				{
					Id = project.Id,
					AssignedUserFullName = string.IsNullOrWhiteSpace(project.AssignedUserEmailAddress) ? null : project.AssignedUserFullName,
					AdvisoryBoardDate = advisoryBoardDecision?.AdvisoryBoardDecisionDetails?.AdvisoryBoardDecisionDate,
					IncomingTrustName = project.TransferringAcademies.FirstOrDefault()?.IncomingTrustName,
					IncomingTrustUkprn = project.TransferringAcademies.FirstOrDefault()?.IncomingTrustUkprn,
					LocalAuthority = localAuthorities,
					OutgoingTrustName = project.OutgoingTrustName,
					ProposedAcademyTransferDate = project.TargetDateForTransfer,
					Region = regions,
					SchoolName = schoolNames,
					SchoolType = schoolTypes,
					Status = project.Status,
					TransferType = project.TypeOfTransfer,
					Urn = project.Urn.ToString(),
				};
			}
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
						return new TransferringAcademiesResponse
						{
							OutgoingAcademyUkprn = ta.OutgoingAcademyUkprn,
							IncomingTrustUkprn = ta.IncomingTrustUkprn,
							IncomingTrustName = ta.IncomingTrustName,
							PupilNumbersAdditionalInformation = ta.PupilNumbersAdditionalInformation,
							LatestOfstedReportAdditionalInformation = ta.LatestOfstedReportAdditionalInformation,
							KeyStage2PerformanceAdditionalInformation = ta.KeyStage2PerformanceAdditionalInformation,
							KeyStage4PerformanceAdditionalInformation = ta.KeyStage4PerformanceAdditionalInformation,
							KeyStage5PerformanceAdditionalInformation = ta.KeyStage5PerformanceAdditionalInformation
						};
					}).ToList()
				};
			});
		}

	}

}
