using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.Service.UnitTest.Mocks;
using Dfe.Academies.Contracts.V4.Establishments;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class TransferProjectQueryServiceTests
	{
		IAcademiesQueryService _establishmentRepo = new Mock<IAcademiesQueryService>().Object;
		IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository = new Mock<IAdvisoryBoardDecisionRepository>().Object;
		private Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private TransferProjectQueryService _service;
		public TransferProjectQueryServiceTests()
		{
			_mockTransferProjectRepository = new Mock<ITransferProjectRepository>();
			_service = new TransferProjectQueryService(_mockTransferProjectRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);
		}

		[Fact]
		public async Task GetByUrn_ShouldReturnExpectedResponse()
		{
			// Arrange
			var dummyUrn = 1;
			var transferringAcademies = new List<TransferringAcademy>() { 
				new TransferringAcademy("23456789", "in trust", "34567890", "", "") ,
				new TransferringAcademy("23456789", "in trust", "34567891", "", "")

			};
			ITransferProject dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"out trust",
				transferringAcademies,
				false,
				DateTime.Now
			);
			_mockTransferProjectRepository.Setup(repo => repo.GetByUrn(It.IsAny<int>())).Returns(Task.FromResult((ITransferProject?)dummyTransferProject));
			var expectedResponse = AcademyTransferProjectResponseFactory.Create(dummyTransferProject);

			// Action
			var result = await _service.GetByUrn(dummyUrn);

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetById_ShouldReturnExpectedResponse()
		{
			// Arrange
			var dummyId = 1;
			var transferringAcademies = new List<TransferringAcademy>() {
				new TransferringAcademy("23456789", "in trust", "34567890", "", "") ,
				new TransferringAcademy("23456789", "in trust", "34567891", "", "")

			};
			var dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"out trust",
				transferringAcademies,
				false,
				DateTime.Now
			);
			_mockTransferProjectRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(Task.FromResult(dummyTransferProject));
			var expectedResponse = AcademyTransferProjectResponseFactory.Create(dummyTransferProject);

			// Action
			var result = await _service.GetById(dummyId);

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjects_ShouldReturnExpectedResponse()
		{
			// Arrange
			var mockAcademiesQueryService = new Mock<IAcademiesQueryService>();
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionGetDataByProjectIdQuery) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionRepository>();

			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();

			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1", "dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			var establishmentDto = new EstablishmentDto
			{
				Ukprn = "dummyOutgoingAcademyUkprn1",
				Urn = "123456",
				Name = "Dummy Academy",
				LocalAuthorityName = "Local Authority1",
				EstablishmentType = new NameAndCodeDto { Name = "Type1" },
				Gor = new NameAndCodeDto { Name = "Region1" }
			};
			_mockTransferProjectRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>()))
						  .ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(service => service.PostBulkEstablishmentsByUkprns(It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync([establishmentDto]);
			var service = new TransferProjectQueryService(
				_mockTransferProjectRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionGetDataByProjectIdQuery.Object
			);
			var dummyProjects = new List<ExportedTransferProjectModel>
			{
				new()
				{
					Id = 0,
					IncomingTrustName = "dummyIncomingTrustName1",
					IncomingTrustUkprn = "dummyIncomingTrustUkprn1",
					OutgoingTrustName = "dummyOutgoingTrustName",
					OutgoingTrustUKPRN = "dummyOutgoingTrustUkprn",
					LocalAuthority = "Local Authority1",
					Region = "Region1",
					SchoolName = "Dummy Academy",
					SchoolType = "Type1",
					Urn = "123456",
					PFI = " "
				}
			};
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 1);

			// Action
			var result = await service.GetExportedTransferProjects([], string.Empty, [], 1, 10);

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjects_ShouldReturnNoResultsWhenFiltered()
		{
			// Arrange
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1","dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			// Mock the setup to return the dummy project
			_mockTransferProjectRepository.Setup(repo => repo.GetAllTransferProjects()).ReturnsAsync(new List<ITransferProject>() { mockTransferProject.Object });

			var dummyProjects = new List<ExportedTransferProjectModel>();
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 0);

			// Action
			var result = await _service.GetExportedTransferProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>());

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjectsWithMultipleAcademies_ShouldReturnExpectedResponse()
		{
			// Arrange
			var mockAcademiesQueryService = new Mock<IAcademiesQueryService>();
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionRepository) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionRepository>();
			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {

				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1", "dummyRegion", "dummyLocalAuthority"),
				new("dummyIncomingTrustUkprn2", "dummyOutgoingAcademyUkprn2", "dummyIncomingTrustName2", "dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			_mockTransferProjectRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));

			mockAdvisoryBoardDecisionRepository
				.Setup(query => query.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
				.ReturnsAsync(mockDecision.Object);

			var establishments = new List<EstablishmentDto>()
			{
				new() { Name = "dummyAcademy1", LocalAuthorityName = "dummyLocalAuthority1", Ukprn = "dummyOutgoingAcademyUkprn1" },
				new() { Name = "dummyAcademy2", Ukprn = "dummyOutgoingAcademyUkprn2" }
			};

			mockAcademiesQueryService.Setup(academiesQueryService => academiesQueryService.PostBulkEstablishmentsByUkprns(It.IsAny<IEnumerable<string>>()))
					.ReturnsAsync(establishments);

			var service = new TransferProjectQueryService(
				_mockTransferProjectRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionRepository.Object
			);

			var dummyProjects = new List<ExportedTransferProjectModel>
			{
				new()
				{
					Id = 0,
					IncomingTrustName = "dummyIncomingTrustName1",
					IncomingTrustUkprn = "dummyIncomingTrustUkprn1",
					OutgoingTrustName = "dummyOutgoingTrustName",
					OutgoingTrustUKPRN = "dummyOutgoingTrustUkprn",
					LocalAuthority = "dummyLocalAuthority1",
					Region = null,
					SchoolName = "dummyAcademy1",
					SchoolType = null,
					Urn = null,
					PFI = " "
				},
				new() {
					Id = 0,
					IncomingTrustName = "dummyIncomingTrustName1",
					IncomingTrustUkprn = "dummyIncomingTrustUkprn1",
					OutgoingTrustName = "dummyOutgoingTrustName",
					OutgoingTrustUKPRN = "dummyOutgoingTrustUkprn",
					LocalAuthority = null,
					Region = null,
					SchoolName = "dummyAcademy2",
					SchoolType = null,
					Urn = null,
					PFI = " "
				}
			};
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 2);

			// Action
			var result = await service.GetExportedTransferProjects(Enumerable.Empty<string>(), null, Enumerable.Empty<string>(), 1, 10);

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetProjects_ReturnsFilteredProjects()
		{
			// Arrange 
			var states = new List<string> { "Active", "Completed" };
			var title = "Project X";
			var deliveryOfficers = new List<string> { "Officer A" };
			var page = 1;
			var count = 10;

			var transferrinAcademies1 = new List<TransferringAcademy>() { new TransferringAcademy("inUkprn1", "In Trust 1", "ukprn1", "", "") };
			var transferrinAcademies2 = new List<TransferringAcademy>() { new TransferringAcademy("inUkprn2", "In Trust 2", "ukprn2", "", "") };
			var dummyProjects = new List<TransferProject>
			{
				TransferProject.Create("outUkprn1", "Out Trust 1", transferrinAcademies1, false, DateTime.UtcNow),
				TransferProject.Create("outUkprn2", "Out Trust 2", transferrinAcademies2, false, DateTime.UtcNow)
			};

			var expectedData = dummyProjects.Select(p => new AcademyTransferProjectSummaryResponse
			{
				ProjectUrn = p.Urn.ToString(),
				ProjectReference = p.ProjectReference!,
				OutgoingTrustUkprn = p.OutgoingTrustUkprn,
				OutgoingTrustName = p.OutgoingTrustName!,
				Status = p.Status!,
				TransferringAcademies = p.TransferringAcademies.Select(a => new TransferringAcademyDto
				{
					IncomingTrustName = a.IncomingTrustName,
					IncomingTrustUkprn = a.IncomingTrustUkprn,
					OutgoingAcademyUkprn = a.OutgoingAcademyUkprn,
					Region = a.Region,
					LocalAuthority = a.LocalAuthority
				}).ToList(),
				AssignedUser = null!,
				IsFormAMat = false
			});

			_mockTransferProjectRepository.Setup(repo => repo.SearchProjects(states, title, deliveryOfficers, page, count))
				.ReturnsAsync((dummyProjects, 2));  // 2 represents the total count of items

			// Action
			var result = await _service.GetProjects(states, title, deliveryOfficers, page, count);

			// Assert
			result.Should().NotBeNull();
			result!.Data.Should().BeEquivalentTo(expectedData);
			result.Paging.Page.Should().Be(page);
			result.Paging.RecordCount.Should().Be(2);
		}

		[Fact]
		public async Task GetTransfersProjectsForGroup_ShouldReturnTranferProjects()
		{
			// Arrange
			var trustUrn = "23456789";
			var dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"out trust",
				[
					new TransferringAcademy(trustUrn, "in trust", "34567890", "", ""),
					new TransferringAcademy(trustUrn, "in trust", "34567891", "", "")
				],
				false,
				DateTime.Now
			);
			var cancelationToken = CancellationToken.None;
			_mockTransferProjectRepository.Setup(repo => repo.GetTransfersProjectsForGroup(trustUrn, cancelationToken))
				.ReturnsAsync([(dummyTransferProject!)]);
			var expectedResponse = _service.AcademyTransferProjectSummaryResponse([dummyTransferProject]);

			// Action
			var result = await _service.GetTransfersProjectsForGroup(trustUrn, cancelationToken);

			// Assert
			result.Should().BeEquivalentTo(expectedResponse);
		}
		
		[Fact]
		public async Task GetTransfersProjectsForGroup_ShouldReturnNoTranferProject()
		{
			// Arrange
			var trustUrn = "123213";
			var cancelationToken = CancellationToken.None;
			_mockTransferProjectRepository.Setup(repo => repo.GetTransfersProjectsForGroup(trustUrn, cancelationToken))
				.ReturnsAsync([]);
			var service = new TransferProjectQueryService(_mockTransferProjectRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);

			// Action
			var result = await service.GetTransfersProjectsForGroup(trustUrn, cancelationToken);

			// Assert
			result.Should().BeEmpty();
		}
	}
}
