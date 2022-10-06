using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.AutoMapper;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.AutoMapper
{
    public class AutoMapperSetupTests
    {
        private MockRepository mockRepository;
		private Fixture fixture;
		private IMapper mapper;

        public AutoMapperSetupTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
			this.fixture = new Fixture();
			this.fixture.Customize(new AutoPopulatedMoqPropertiesCustomization());			
			
			var mapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperProfile>();
			});

			// checks the config is valid
			mapperConfig.AssertConfigurationIsValid();

			mapper =  new Mapper(mapperConfig);
		}

        [Fact]
        public void CanMap_JoinTrust_MapFromDomainToState()
        {
            // Arrange
			var joinTrustDomainObj = this.fixture.Create<IJoinTrust>();

			Mock.Get(joinTrustDomainObj).Setup(x => x.Id).Returns(10101);		
			Mock.Get(joinTrustDomainObj).Setup(x => x.UKPRN).Returns(295061);
			Mock.Get(joinTrustDomainObj).Setup(x => x.TrustName).Returns("Test Trust");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrust).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("it has changed");

			// Act
			var result = mapper.Map<JoinTrustState>(joinTrustDomainObj);

            // Assert
            Assert.NotNull(result);
			result.Should().BeEquivalentTo(joinTrustDomainObj);
        }

		[Fact]
		public void CanMap_JoinTrust_MapFromStateToDomain()
		{
			// Arrange
			var joinTrustStateObj = this.fixture.Create<JoinTrustState>();

			joinTrustStateObj.Id = 10101;
			joinTrustStateObj.UKPRN = 295061;
			joinTrustStateObj.TrustName = "Test Trust";
			joinTrustStateObj.ChangesToTrust = true;
			joinTrustStateObj.ChangesToTrustExplained = "it has changed";

			// Act
			var result = mapper.Map<JoinTrust>(joinTrustStateObj);

			// Assert
			Assert.NotNull(result);
			result.Should().BeEquivalentTo(joinTrustStateObj, cfg => cfg.Excluding(x => x.CreatedOn).Excluding(x => x.LastModifiedOn));
		}

		[Fact]
		public void CanMap_JoinTrust_MapFromDomainToServiceModel()
		{
			// Arrange
			var joinTrustDomainObj = this.fixture.Create<IJoinTrust>();

			Mock.Get(joinTrustDomainObj).Setup(x => x.Id).Returns(10101);
			Mock.Get(joinTrustDomainObj).Setup(x => x.UKPRN).Returns(295061);
			Mock.Get(joinTrustDomainObj).Setup(x => x.TrustName).Returns("Test Trust");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrust).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("it has changed");

			// Act
			var result = mapper.Map<ApplicationJoinTustServiceModel>(joinTrustDomainObj);

			// Assert
			Assert.NotNull(result);
			result.Should().BeEquivalentTo(joinTrustDomainObj);
		}
	}
}
