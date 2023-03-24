using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate
{
	public class ApplicationSchoolAdditionalDetailsTests
	{
		private readonly Fixture _fixture;

		public ApplicationSchoolAdditionalDetailsTests()
		{
			_fixture = new Fixture();
		}

		[Fact]
		public void SetAdditionalDetails__ReturnsSuccess()
		{
			
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Build<School>().With(x=> x.Id, 2).Create()
				},
				null,
				null,
				null);

			var result = subject.SetAdditionalDetails(
				2,
				"str",
				null,
				false,
				null,
				null,
				null,
				null,
				false,
				null,
				null,
				DateTimeOffset.Now,
				"str",
				null,
				SchoolEqualitiesProtectedCharacteristics.Unlikely,
				"str");

			Assert.IsType<CommandSuccessResult>(result);
		}
		
		[Fact]
		public void SetAdditionalDetails__ReturnsNotFound()
		{
			
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Build<School>().With(x=> x.Id, 2).Create()
				},
				null,
				null,
				null);

			var result = subject.SetAdditionalDetails(
				5,
				"str",
				null,
				false,
				null,
				null,
				null,
				null,
				false,
				null,
				null,
				DateTimeOffset.Now,
				"str",
				null,
				SchoolEqualitiesProtectedCharacteristics.Unlikely,
				"str");

			Assert.IsType<NotFoundCommandResult>(result);
		}
	}
}
