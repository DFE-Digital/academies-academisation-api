using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate
{
	public class ApplicationSubmitTests
	{
		private readonly Fixture _fixture = new();

		[Fact]
		public void ApplicationStatusIsSubmitted___ValidationError()
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.Submitted,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, SchoolDetails>());

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandValidationErrorResult>(result);
		}

		[Fact]
		public void ApplicationStatusIsInProgress___Success()
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, SchoolDetails>());

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
		}

		[Fact]
		public void ApplicationTypeIsJoinAMat_MoreThanOneSchool___ValidationError()
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.JoinAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, SchoolDetails>
				{
					{ 1, _fixture.Create<SchoolDetails>() },
					{ 2, _fixture.Create<SchoolDetails>() }
				});

			// act
			var result = subject.Submit();

			// assert
			DfeAssert.CommandValidationError(result, nameof(Application.Schools));
		}


		[Fact]
		public void ApplicationTypeIsJoinAMat_OneSchool___Success()
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.JoinAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, SchoolDetails>
				{
					{ 1, _fixture.Create<SchoolDetails>() },
				});

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
		}

	}
}
