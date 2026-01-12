using System;
using System.Collections.Generic;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.Services;

public class ApplicationSubmissionServiceTests
{
	private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
	private readonly DateTime _ApplicationSubmittedDate = DateTime.UtcNow;
	private readonly Mock<IEnumerable<EstablishmentDto>> _mockEstablishmentDtos = new();

	public ApplicationSubmissionServiceTests()
	{
		_mockDateTimeProvider.Setup(x => x.Now).Returns(_ApplicationSubmittedDate);
	}

	[Fact]
	public void ApplicationSubmitValidationError___ProjectNotCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new(); 
		mockApplication.Setup(a => a.Submit(It.Is<DateTime>(x => x == _ApplicationSubmittedDate))).Returns(new CommandValidationErrorResult(new List<ValidationError>()));

		ApplicationSubmissionService subject = new(mockProjectFactory.Object, _mockDateTimeProvider.Object);

		// act
		subject.SubmitApplication(mockApplication.Object, _mockEstablishmentDtos.Object);

		// assert
		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object, _mockEstablishmentDtos.Object), Times.Never);
	}

	[Fact]
	public void ApplicationTypeJoinAMat___ApplicationSubmitted_ProjectCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new();
		mockApplication.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		mockApplication.Setup(a => a.Submit(It.Is<DateTime>(x => x == _ApplicationSubmittedDate))).Returns(new CommandSuccessResult());

		ApplicationSubmissionService subject = new(mockProjectFactory.Object, _mockDateTimeProvider.Object);

		// act
		subject.SubmitApplication(mockApplication.Object, _mockEstablishmentDtos.Object);

		// assert
		mockApplication.Verify(a => a.Submit(It.Is<DateTime>(x => x == _ApplicationSubmittedDate)), Times.Once);

		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object, _mockEstablishmentDtos.Object), Times.Once);
	}

	[Fact]
	public void ApplicationTypeFormAMat___ApplicationSubmitted_ProjectCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new(); 
		mockApplication.SetupGet(a => a.ApplicationType).Returns(ApplicationType.FormAMat);
		mockApplication.Setup(a => a.Submit(It.Is<DateTime>(x => x == _ApplicationSubmittedDate))).Returns(new CommandSuccessResult());

		ApplicationSubmissionService subject = new(mockProjectFactory.Object, _mockDateTimeProvider.Object);

		// act
		subject.SubmitApplication(mockApplication.Object, _mockEstablishmentDtos.Object);

		// assert
		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object, _mockEstablishmentDtos.Object), Times.Never);
	}
}
