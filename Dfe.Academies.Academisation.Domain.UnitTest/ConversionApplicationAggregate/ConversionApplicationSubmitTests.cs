using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
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
			ConversionApplication subject = new(1, ApplicationType.FormAMat, new Dictionary<int, ContributorDetails>(), ApplicationStatus.Submitted);

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandValidationErrorResult>(result);
		}

		[Fact]
		public void ApplicationStatusIsInProgress___Success()
		{
			// arrange
			ConversionApplication subject = new(1, ApplicationType.FormAMat, new Dictionary<int, ContributorDetails>(), ApplicationStatus.InProgress);

			// act
			var result = subject.Submit();

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal(ApplicationStatus.Submitted, subject.ApplicationStatus);
		}
	}
}
