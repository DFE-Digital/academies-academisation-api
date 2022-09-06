using System;
using Dfe.Academies.Academisation.Data.ConversionLegalRequirement;
using Xunit;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.LegalRequirements;

namespace Dfe.Academies.Academisation.Data.UnitTest.LegalRequirement
{
	public class LegalRequirementStateTests
	{
		[Fact]
		public void WhenRecordExists___ShouldReturnExpectedLegalRequirementObject()
		{
			// Arrange
			var now = DateTime.UtcNow;
			var sut = new Fixture().Build<LegalRequirementState>()
				.With(lr => lr.LastModifiedOn, now)
				.With(lr => lr.CreatedOn, now)
				.Create();

			var expectedDetails = new LegalRequirementDetails(
				sut.ProjectId, sut.HaveProvidedResolution, sut.HadConsultation, sut.HasDioceseConsented, sut.HasFoundationConsented, sut.IsSectionComplete);

			// Act
			var result = sut.MapToDomain();

			// Assert
			Assert.Multiple(
				() => Assert.Equivalent(expectedDetails, result.LegalRequirementDetails),
				() => Assert.Equal(now, result.CreatedOn),
				() => Assert.Equal(now, result.LastModifiedOn),
				() => Assert.NotEqual(default, result.Id)
			);
		}		
	}
}
