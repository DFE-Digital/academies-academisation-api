using Xunit;
using Moq;
using FluentAssertions;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academies.Academisation.Service.UnitTest.Mocks;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class TransferProjectQueryServiceTests
	{
		[Fact]
		public async Task GetByUrn_ShouldReturnExpectedResponse()
		{
			// Mocking the Dependencies
			var mockRepository = new Mock<ITransferProjectRepository>();

			var dummyUrn = 1;

			// Create a TransferProject
			ITransferProject dummyTransferProject = TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"dummyIncomingTrustUkprn",
				new List<string> { "dummyUkprn1", "dummyUkprn2" },
				DateTime.Now
			);

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetByUrn(It.IsAny<int>())).Returns(Task.FromResult(dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, null, null, null);

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
				"dummyIncomingTrustUkprn",
				new List<string> { "dummyUkprn1", "dummyUkprn2" },
				DateTime.Now
			);

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(Task.FromResult(dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, null, null, null);

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
			mockRepository.Setup(repo => repo.GetAllTransferProjects()).ReturnsAsync(new List<ITransferProject>() { mockTransferProject.Object });

			// Set up behavior for methods
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.Execute(It.IsAny<int>(), false))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn(It.IsAny<string>()))
				.ReturnsAsync(It.IsAny<ExportedEstablishment>());

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				null,
				mockAcademiesQueryService.Object,
				mockServiceScopeFactory.Object
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
			var result = await service.GetExportedTransferProjects(null);

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

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				null,
				null,
				null
			);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>();
			var expectedResponse = new PagedResultResponse<ExportedTransferProjectModel>(dummyProjects, 0);

			// Testing the Method
			var result = await service.GetExportedTransferProjects("TitleFilter");

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
			mockRepository.Setup(repo => repo.GetAllTransferProjects()).ReturnsAsync(new List<ITransferProject>() { mockTransferProject.Object });

			// Set up behavior for methods
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.Execute(It.IsAny<int>(), false))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn("dummyOutgoingAcademyUkprn1"))
				.ReturnsAsync(new ExportedEstablishment() { EstablishmentName = "dummyAcademy1", LocalAuthorityName = "dummyLocalAuthority1" });
			mockAcademiesQueryService
				.Setup(academiesQueryService => academiesQueryService.GetEstablishmentByUkprn("dummyOutgoingAcademyUkprn2"))
				.ReturnsAsync(new ExportedEstablishment() { EstablishmentName = "dummyAcademy2" });

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				null,
				mockAcademiesQueryService.Object,
				mockServiceScopeFactory.Object
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
			var result = await service.GetExportedTransferProjects(null);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}

		private ExportedTransferProjectModel GetDummyTransferProjectModel(
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
	}
}
