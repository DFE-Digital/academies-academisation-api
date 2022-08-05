using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using System.Collections.Generic;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionApplicationAggregate
{
	public class ConversionApplicationSubmitTests
	{
		[Fact]
		public void ApplicationStatusIsSubmitted___ValidationError()
		{
			// arrange
			Application subject = new(
				1,
				ApplicationType.FormAMat,
				ApplicationStatus.Submitted,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, ApplicationSchoolDetails>());

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
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new Dictionary<int, ApplicationSchoolDetails>());

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
		}
	}
}
