using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using AutoMapper;
using Dfe.Academies.Academisation.Data.Migrations;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.Application;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TransferProjectQueryService : ITransferProjectQueryService
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IMapper _mapper;

		public TransferProjectQueryService(ITransferProjectRepository transferProjectRepository, IMapper mapper)
		{
			_transferProjectRepository = transferProjectRepository;
			_mapper = mapper;
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

			projects = projects
			// remove any projects without an incoming or outgoing trust.
			.Where(p =>
				!string.IsNullOrEmpty(p.OutgoingTrustUkprn) && !string.IsNullOrEmpty(p.OutgoingTrustName) &&
				!p.TransferringAcademies.Any(ta => string.IsNullOrEmpty(ta.IncomingTrustUkprn) || string.IsNullOrEmpty(ta.IncomingTrustName))).ToList();
			
			var recordTotal = projects.Count();

			projects = projects.OrderByDescending(atp => atp.ProjectUrn)
			.Skip((page - 1) * count).Take(count).ToList();

			return await Task.FromResult(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(projects, recordTotal));
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
