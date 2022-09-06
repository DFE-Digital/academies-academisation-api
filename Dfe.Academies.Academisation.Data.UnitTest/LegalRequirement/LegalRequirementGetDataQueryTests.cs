using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.LegalRequirement;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.LegalRequirement
{
	public class LegalRequirementGetDataQueryTests
	{
		private readonly LegalRequirementGetDataQuery _target;

		public LegalRequirementGetDataQueryTests()
		{
			var mockContext = new TestLegalRequirementContext().CreateContext();
			_target = new(mockContext);
		}

		[Fact]
		public async Task WhenRecordExists___ShouldReturnExpectedConversionAdvisoryBoardDecision()
		{
			//Arrange			
			var expectedProjectId = 1;

			//Act
			var result = await _target.Execute(expectedProjectId);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.IsType<Domain.LegalRequirement.LegalRequirement>(result),
				() => Assert.Equal(expectedProjectId, result!.LegalRequirementDetails.ProjectId),
				() => Assert.NotEqual(default, result!.Id),
				() => Assert.NotEqual(default, result!.CreatedOn),
				() => Assert.NotEqual(default, result!.LastModifiedOn)
			);
		}

		[Fact]
		public async Task WhenRecordDoesNotExist___ShouldReturnNull()
		{
			//Arrange
			const int id = 4;

			//Act
			var result = await _target.Execute(id);

			//Assert
			Assert.Null(result);
		}
	}
}
