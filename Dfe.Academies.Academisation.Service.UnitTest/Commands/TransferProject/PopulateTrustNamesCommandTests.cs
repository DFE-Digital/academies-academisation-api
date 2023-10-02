using FluentAssertions;
using Moq;
using System;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
    public class PopulateTrustNamesCommandTests
    {
        private MockRepository mockRepository;



        public PopulateTrustNamesCommandTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private PopulateTrustNamesCommand CreatePopulateTrustNamesCommand()
        {
            return new PopulateTrustNamesCommand();
        }

        [Fact]
        public void CanConstruct()
        {
            // Arrange
            var populateTrustNamesCommand = this.CreatePopulateTrustNamesCommand();


			// Assert
			populateTrustNamesCommand.Should().NotBeNull();
        }
    }
}
