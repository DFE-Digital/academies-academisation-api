using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.Service.Queries;

public class TransferProjectQueryServiceTests
{
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

		var service = new TransferProjectQueryService(mockRepository.Object, null);

		// Setting up Test Data
		var expectedResponse = AcademyTransferProjectResponseFactory.Create(dummyTransferProject);

		// Testing the Method
		var result = await service.GetById(dummyId);

		// Assertion
		result.Should().BeEquivalentTo(expectedResponse);
	}
}
