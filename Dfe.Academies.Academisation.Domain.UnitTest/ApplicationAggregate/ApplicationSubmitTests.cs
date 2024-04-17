using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate
{
	public class ApplicationSubmitTests
	{
		private readonly Fixture _fixture = new();

		public ApplicationSubmitTests()
		{
			_fixture.Customize(new AutoMoqCustomization());
		}

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
				new List<School>(),
				_fixture.Create<JoinTrust>(),
				null);

			// act
			var result = subject.Submit(DateTime.UtcNow);

			// assert
			Assert.IsType<CommandValidationErrorResult>(result);
		}

		[Theory]
		[InlineData(ApplicationType.FormASat)]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public void NoSchools___ValidationError(ApplicationType applicationType)
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				applicationType,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>(),
				_fixture.Create<JoinTrust>(),
				null);

			// act
			var result = subject.Submit(DateTime.UtcNow);

			// assert
			DfeAssert.CommandValidationError(result, nameof(Application.Schools));
		}

		[Theory]
		[InlineData(ApplicationType.FormASat)]
		[InlineData(ApplicationType.JoinAMat)]
		public void ApplicationTypeIsJoinAMatOrFormASat_MoreThanOneSchool___ValidationError(ApplicationType applicationType)
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				applicationType,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Create<School>(),
					_fixture.Create<School>()
				},
				_fixture.Create<JoinTrust>(),
				null);

			// act
			var result = subject.Submit(DateTime.UtcNow);

			// assert
			DfeAssert.CommandValidationError(result, nameof(Application.Schools));
		}

		[Theory]
		[InlineData(ApplicationType.FormASat)]
		[InlineData(ApplicationType.JoinAMat)]
		public void ApplicationTypeIsJoinAMatOrFormASat_OneSchool___Success(ApplicationType applicationType)
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				applicationType,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					 _fixture.Create<School>()
				},
				_fixture.Create<JoinTrust>(),
				null);
			var submittedDate = DateTime.UtcNow;

			// act
			var result = subject.Submit(submittedDate);

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
			Assert.Equal(submittedDate, subject.ApplicationSubmittedDate);
		}

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		public void ApplicationTypeIsFormAMat_TwoSchools___Success(ApplicationType applicationType)
		{
			// arrange
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				applicationType,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Create<School>(),
					_fixture.Create<School>()
				},
				_fixture.Create<JoinTrust>(),
				null);

			var submittedDate = DateTime.UtcNow;

			// act
			var result = subject.Submit(submittedDate);

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
			Assert.Equal(submittedDate, subject.ApplicationSubmittedDate);
		}
	}
}
