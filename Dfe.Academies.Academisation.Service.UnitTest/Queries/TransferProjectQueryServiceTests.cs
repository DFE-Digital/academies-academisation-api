﻿using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
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
		IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository = new Mock<IAdvisoryBoardDecisionRepository>().Object;
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


			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetByUrn(It.IsAny<int>())).Returns(Task.FromResult((ITransferProject?)dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);

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

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(Task.FromResult(dummyTransferProject));

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);

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
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionGetDataByProjectIdQuery) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionRepository>();

			// Mocking the data
			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();

			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1", "dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			// Mock establishment data
			var establishmentDto = new EstablishmentDto
			{
				Ukprn = "dummyOutgoingAcademyUkprn1",
				Urn = "123456",
				Name = "Dummy Academy",
				LocalAuthorityName = "Local Authority1",
				EstablishmentType = new NameAndCodeDto { Name = "Type1" },
				Gor = new NameAndCodeDto { Name = "Region1" }
			};


			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>()))
						  .ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));

			// Set up behavior for methods
			mockAdvisoryBoardDecisionGetDataByProjectIdQuery
				.Setup(query => query.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
				.ReturnsAsync(mockDecision.Object);

			mockAcademiesQueryService
				.Setup(service => service.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(new List<EstablishmentDto> { establishmentDto });

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionGetDataByProjectIdQuery.Object
			);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>
	{
		new ExportedTransferProjectModel
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
				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1","dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.GetAllTransferProjects()).ReturnsAsync(new List<ITransferProject>() { mockTransferProject.Object });

			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);

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
			var (mockServiceScopeFactory, mockAdvisoryBoardDecisionRepository) = MockServiceScopeFactory.CreateMock<IAdvisoryBoardDecisionRepository>();

			// Mocking the data
			var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
			var mockTransferProject = new MockTransferProject("dummyOutgoingTrustUkprn", "dummyOutgoingTrustName", new List<MockTransferAcademyRecord>() {

				new("dummyIncomingTrustUkprn1", "dummyOutgoingAcademyUkprn1", "dummyIncomingTrustName1", "dummyRegion", "dummyLocalAuthority"),
				new("dummyIncomingTrustUkprn2", "dummyOutgoingAcademyUkprn2", "dummyIncomingTrustName2", "dummyRegion", "dummyLocalAuthority")
			}).CreateMock();

			// Mock the setup to return the dummy project
			mockRepository.Setup(repo => repo.SearchProjects(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((new List<ITransferProject>() { mockTransferProject.Object }, 1));

			// Set up behavior for methods
			mockAdvisoryBoardDecisionRepository
				.Setup(query => query.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
				.ReturnsAsync(mockDecision.Object);

			List<EstablishmentDto> establishments = new List<EstablishmentDto>()
	{
		new EstablishmentDto() { Name = "dummyAcademy1", LocalAuthorityName = "dummyLocalAuthority1", Ukprn = "dummyOutgoingAcademyUkprn1" },
		new EstablishmentDto() { Name = "dummyAcademy2", Ukprn = "dummyOutgoingAcademyUkprn2" }
	};

			mockAcademiesQueryService.Setup(academiesQueryService => academiesQueryService.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>()))
					.ReturnsAsync(establishments);

			var service = new TransferProjectQueryService(
				mockRepository.Object,
				mockAcademiesQueryService.Object,
				mockAdvisoryBoardDecisionRepository.Object
			);

			// Setting up Test Data
			var dummyProjects = new List<ExportedTransferProjectModel>
	{
		new ExportedTransferProjectModel
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
		new ExportedTransferProjectModel
		{
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

			// Testing the Method
			var result = await service.GetExportedTransferProjects(Enumerable.Empty<string>(), null, Enumerable.Empty<string>(), 1, 10);

			// Assertion
			result.Should().BeEquivalentTo(expectedResponse);
		}


		[Fact]
		public async Task GetProjects_ReturnsFilteredProjects()
		{
			// Arrange
			var mockRepository = new Mock<ITransferProjectRepository>();
			var service = new TransferProjectQueryService(mockRepository.Object, _establishmentRepo, _advisoryBoardDecisionRepository);

			var states = new List<string> { "Active", "Completed" };
			var title = "Project X";
			var deliveryOfficers = new List<string> { "Officer A" };
			var page = 1;
			var count = 10;

			// Sample data setup
			var transferrinAcademies1 = new List<TransferringAcademy>() { new TransferringAcademy("inUkprn1", "In Trust 1", "ukprn1", "", "") };
			var transferrinAcademies2 = new List<TransferringAcademy>() { new TransferringAcademy("inUkprn2", "In Trust 2", "ukprn2", "", "") };
			var dummyProjects = new List<TransferProject>
	{
		TransferProject.Create("outUkprn1", "Out Trust 1", transferrinAcademies1, false, DateTime.UtcNow),
		TransferProject.Create("outUkprn2", "Out Trust 2", transferrinAcademies2, false, DateTime.UtcNow)
	};

			// Expected data setup
			var expectedData = dummyProjects.Select(p => new AcademyTransferProjectSummaryResponse
			{
				ProjectUrn = p.Urn.ToString(),
				ProjectReference = p.ProjectReference,
				OutgoingTrustUkprn = p.OutgoingTrustUkprn,
				OutgoingTrustName = p.OutgoingTrustName,
				Status = p.Status,
				TransferringAcademies = p.TransferringAcademies.Select(a => new TransferringAcademyDto
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
