using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.Service.UnitTest.Mocks;
using Dfe.Academies.Contracts.V4.Establishments;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class TransferProjectQueryServiceTests
	{
		IAcademiesQueryService _establishmentRepo = new Mock<IAcademiesQueryService>().Object;
		IAdvisoryBoardDecisionGetDataByProjectIdQuery _advisoryBoardDecisionGetDataByProjectIdQuery = new Mock<IAdvisoryBoardDecisionGetDataByProjectIdQuery>().Object;
		IServiceScopeFactory _serviceScope = new Mock<IServiceScopeFactory>().Object;

		public TransferProjectQueryServiceTests()
		{

		}

		[Fact]
		public async Task GetByUrn_ShouldReturnExpectedResponse()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();

			var dummyUrn = 1;


			// Create a TransferProject
			ITransferProject dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"out trust",
				"dummyIncomingTrustUkprn",
				"in trust",
				new List<string> { "dummyUkprn1", "dummyUkprn2" },
				false,
				DateTime.Now
			);


			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetByUrn(It.IsAny<int>())).Returns(Task.FromResult((ITransferProject?)dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionGetDataByProjectIdQuery);

			// Setting up Test Data
			var expectedResponse = AcademyTransferProjectResponseFactory.Create(dummyTransferProject);

			// Testing the Method
			var result = await service.GetByUrn(dummyUrn);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetById_ShouldReturnExpectedResponse()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();

			var dummyId = 1;


			// Create a TransferProject
			var dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"out trust",
				"dummyIncomingTrustUkprn",
				"in trust",
				new List<string> { "dummyUkprn1", "dummyUkprn2" },
				false,
				DateTime.Now
			);

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(Task.FromResult(dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionGetDataByProjectIdQuery);

			// Setting up Test Data
			var expectedResponse = AcademyTransferProjectResponseFactory.Create(dummyTransferProject);

			// Testing the Method
			var result = await service.GetById(dummyId);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjects_ShouldReturnExpectedResponse()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();
			var mockAcademiesQueryService = new Mock<IAcademiesQueryService>();
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionGetDataByProjectIdQuery) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionGetDataByProjectIdQuery>();

			// Mocking the data
			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1")
			}).CreateMock();



			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));

			// Set up behavior for methods
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.Execute(It.IsAny<int>(), false))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn(It.IsAny<string>()))
				.ReturnsAsync(It.IsAny<EstablishmentDto>());

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionGetDataByProjectIdQuery.Object
			);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>
			{
				GetDummyTransferProjectModel(
					"dummyIncomingTrustName1",
					"dummyIncomingTrustUkprn1",
					"dummyOutgoingTrustName"
				)
			};
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 1);

			// Testing the Method
			var result = await service.GetExportedTransferProjects(Enumerable.Empty<string>(), string.Empty, Enumerable.Empty<string>(), 1, 10);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjects_ShouldReturnNoResultsWhenFiltered()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();

			// Mocking the data
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1")
			}).CreateMock();

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetAllTransferProjects()).ReturnsAsync(new List<ITransferProject>() { mockTransferProject.Object });

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionGetDataByProjectIdQuery);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>();
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 0);

			// Testing the Method
			var result = await service.GetExportedTransferProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>());

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task GetExportedTransferProjectsWithMultipleAcademies_ShouldReturnExpectedResponse()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();
			var mockAcademiesQueryService = new Mock<IAcademiesQueryService>();
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionGetDataByProjectIdQuery) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionGetDataByProjectIdQuery>();

			// Mocking the data
			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1"),
				new("dummyIncomingTrustUkprn2", "dummyOutgoingAcademyUkprn2", "dummyIncomingTrustName2")
			}).CreateMock();

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));

			// Set up behavior for methods
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.Execute(It.IsAny<int>(), false))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn("dummyOutgoingAcademyUkprn1"))
				.ReturnsAsync(new EstablishmentDto() { Name = "dummyAcademy1", LocalAuthorityName = "dummyLocalAuthority1" });
			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn("dummyOutgoingAcademyUkprn2"))
				.ReturnsAsync(new EstablishmentDto() { Name = "dummyAcademy2" });

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionGetDataByProjectIdQuery.Object
			);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>
			{
				GetDummyTransferProjectModel(
					"dummyIncomingTrustName1",
					"dummyIncomingTrustUkprn1",
					"dummyOutgoingTrustName",
					"dummyAcademy1, dummyAcademy2",
					"dummyLocalAuthority1"
				)
			};
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 1);

			// Testing the Method
			var result = await service.GetExportedTransferProjects(Enumerable.Empty<string>(), null, Enumerable.Empty<string>(), 1, 10);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		private static ExportedTransferProjectModel GetDummyTransferProjectModel(
			string incomingTrustName,
			string incomingTrustUkprn,
			string outgoingTrustName,
			string schoolName = "",
			string localAuthority = ""
		)
		{
			return new()
			{
				IncomingTrustName = incomingTrustName,
				IncomingTrustUkprn = incomingTrustUkprn,
				OutgoingTrustName = outgoingTrustName,
				SchoolName = schoolName,
				LocalAuthority = localAuthority,
				Region = "",
				SchoolType = "",
				Urn = "0"
			};
		}


		[Fact]
		public async Task GetProjects_ReturnsFilteredProjects()
		{
			// Arrange
			var mockRepository = new Mock<ITransferProjectRepository>();
			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionGetDataByProjectIdQuery);

			var states = new List<string> { "Active", "Completed" };
			var title = "Project X";
			var deliveryOfficers = new List<string> { "Officer A" };
			var page = 1;
			var count = 10;

			// Sample data setup
			var dummyProjects = new List<TransferProject>
	{
		TransferProject.Create("outUkprn1", "Out Trust 1", "inUkprn1", "In Trust 1", new List<string> { "ukprn1" },false, DateTime.UtcNow),
		TransferProject.Create("outUkprn2", "Out Trust 2", "inUkprn2", "In Trust 2", new List<string> { "ukprn2" },false, DateTime.UtcNow)
	};

			// Expected data setup
			var expectedData = dummyProjects.Select(p => new AcademyTransferProjectSummaryResponse
			{
				ProjectUrn = p.Urn.ToString(),
				ProjectReference = p.ProjectReference,
				OutgoingTrustUkprn = p.OutgoingTrustUkprn,
				OutgoingTrustName = p.OutgoingTrustName,
				Status = p.Status,
				TransferringAcademies = p.TransferringAcademies.Select(a => new TransferringAcademiesResponse
				{
					IncomingTrustName = a.IncomingTrustName,
					IncomingTrustUkprn = a.IncomingTrustUkprn,
					OutgoingAcademyUkprn = a.OutgoingAcademyUkprn,
				}).ToList(),
				AssignedUser = null,
				IsFormAMat = false
			});

			mockRepository.Setup(repo => repo.SearchProjects(states, title, deliveryOfficers, page, count))
				.ReturnsAsync((dummyProjects, 2));  // 2 represents the total count of items

			// Act
			var result = await service.GetProjects(states, title, deliveryOfficers, page, count);

			// Assert
			result.Should().NotBeNull();
			result!.Data.Should().BeEquivalentTo(expectedData);
			result.Paging.Page.Should().Be(page);
			result.Paging.RecordCount.Should().Be(2);
		}

	}
}
