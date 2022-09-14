﻿using System.Collections.Generic;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.Services;

public class ApplicationSubmissionServiceTests
{
	[Fact]
	public void ApplicationSubmitValidationError___ProjectNotCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new();
		mockApplication.Setup(a => a.Submit()).Returns(new CommandValidationErrorResult(new List<ValidationError>()));

		ApplicationSubmissionService subject = new(mockProjectFactory.Object);

		// act
		subject.SubmitApplication(mockApplication.Object);

		// assert
		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object), Times.Never);
	}

	[Fact]
	public void ApplicationTypeJoinAMat___ApplicationSubmitted_ProjectCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new();
		mockApplication.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		mockApplication.Setup(a => a.Submit()).Returns(new CommandSuccessResult());

		ApplicationSubmissionService subject = new(mockProjectFactory.Object);

		// act
		subject.SubmitApplication(mockApplication.Object);

		// assert
		mockApplication.Verify(a => a.Submit(), Times.Once);

		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object), Times.Once);
	}

	[Fact]
	public void ApplicationTypeFormAMat___ApplicationSubmitted_ProjectCreated()
	{
		// arrange
		Mock<IProjectFactory> mockProjectFactory = new();

		Mock<IApplication> mockApplication = new();
		mockApplication.SetupGet(a => a.ApplicationType).Returns(ApplicationType.FormAMat);
		mockApplication.Setup(a => a.Submit()).Returns(new CommandSuccessResult());

		ApplicationSubmissionService subject = new(mockProjectFactory.Object);

		// act
		subject.SubmitApplication(mockApplication.Object);

		// assert
		mockProjectFactory.Verify(pf => pf.Create(
			mockApplication.Object), Times.Never);
	}
}
