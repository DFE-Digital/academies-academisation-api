using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
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
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("ChangesToTrustExplained it has changed");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernance).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernanceExplained).Returns("ChangesToLaGovernanceExplained it has changed");

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
			joinTrustStateObj.ChangesToTrustExplained = "ChangesToTrustExplained it has changed";
			joinTrustStateObj.ChangesToLaGovernance = true;
			joinTrustStateObj.ChangesToLaGovernanceExplained = "ChangesToLaGovernanceExplained it has changed";

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
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("ChangesToTrustExplained it has changed");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernance).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernanceExplained).Returns("ChangesToLaGovernanceExplained it has changed");

			// Act
			var result = mapper.Map<ApplicationJoinTrustServiceModel>(joinTrustDomainObj);

			// Assert
			Assert.NotNull(result);
			result.Should().BeEquivalentTo(joinTrustDomainObj);
		}

		[Fact]
		public void CanMap_FormTrust_MapFromDomainToState()
		{
			// Arrange
			var formTrustDomainObj = this.fixture.Create<IFormTrust>();
			// relying on the all details been set here by autofixture
			var trustDetails = this.fixture.Create<FormTrustDetails>();

			Mock.Get(formTrustDomainObj).Setup(x => x.Id).Returns(10101);
			Mock.Get(formTrustDomainObj).Setup(x => x.TrustDetails).Returns(trustDetails);

			// Act
			var result = mapper.Map<FormTrustState>(formTrustDomainObj);

			// Assert
			Assert.NotNull(result);
			result.FormTrustImprovementSupport.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustImprovementSupport);
			result.FormTrustImprovementApprovedSponsor.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustImprovementApprovedSponsor);
			result.FormTrustProposedNameOfTrust.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustProposedNameOfTrust);
			result.TrustApproverName.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.TrustApproverName);
			result.FormTrustGrowthPlansYesNo.Should().Be(formTrustDomainObj.TrustDetails.FormTrustGrowthPlansYesNo);
			result.FormTrustImprovementApprovedSponsor.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustImprovementApprovedSponsor);
			result.FormTrustImprovementStrategy.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustImprovementStrategy);
			result.FormTrustOpeningDate.Should().Be(formTrustDomainObj.TrustDetails.FormTrustOpeningDate);
			result.FormTrustPlanForGrowth.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustPlanForGrowth);
			result.FormTrustPlansForNoGrowth.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustPlansForNoGrowth);
			result.FormTrustProposedNameOfTrust.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustProposedNameOfTrust);
			result.FormTrustReasonApprovaltoConvertasSAT.Should().Be(formTrustDomainObj.TrustDetails.FormTrustReasonApprovaltoConvertasSAT);
			result.FormTrustReasonApprovedPerson.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonApprovedPerson);
			result.FormTrustReasonForming.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonForming);
			result.FormTrustReasonFreedom.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonFreedom);
			result.FormTrustReasonGeoAreas.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonGeoAreas);
			result.FormTrustReasonImproveTeaching.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonImproveTeaching);
			result.FormTrustReasonVision.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.FormTrustReasonVision);
			result.TrustApproverEmail.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.TrustApproverEmail);
			result.TrustApproverName.Should().BeEquivalentTo(formTrustDomainObj.TrustDetails.TrustApproverName);
			result.Id.Should().Be(formTrustDomainObj.Id);
		}

		[Fact]
		public void CanMap_FormTrust_MapFromStateToDomain()
		{
			// Arrange
			var formTrustStateObj = this.fixture.Create<FormTrustState>();

			// Act
			var result = mapper.Map<FormTrust>(formTrustStateObj);

			// Assert
			Assert.NotNull(result);
			result.TrustDetails.FormTrustImprovementSupport.Should().BeEquivalentTo(formTrustStateObj.FormTrustImprovementSupport);
			result.TrustDetails.FormTrustImprovementApprovedSponsor.Should().BeEquivalentTo(formTrustStateObj.FormTrustImprovementApprovedSponsor);
			result.TrustDetails.FormTrustProposedNameOfTrust.Should().BeEquivalentTo(formTrustStateObj.FormTrustProposedNameOfTrust);
			result.TrustDetails.TrustApproverName.Should().BeEquivalentTo(formTrustStateObj.TrustApproverName);
			result.TrustDetails.FormTrustGrowthPlansYesNo.Should().Be(formTrustStateObj.FormTrustGrowthPlansYesNo);
			result.TrustDetails.FormTrustImprovementApprovedSponsor.Should().BeEquivalentTo(formTrustStateObj.FormTrustImprovementApprovedSponsor);
			result.TrustDetails.FormTrustImprovementStrategy.Should().BeEquivalentTo(formTrustStateObj.FormTrustImprovementStrategy);
			result.TrustDetails.FormTrustOpeningDate.Should().Be(formTrustStateObj.FormTrustOpeningDate);
			result.TrustDetails.FormTrustPlanForGrowth.Should().BeEquivalentTo(formTrustStateObj.FormTrustPlanForGrowth);
			result.TrustDetails.FormTrustPlansForNoGrowth.Should().BeEquivalentTo(formTrustStateObj.FormTrustPlansForNoGrowth);
			result.TrustDetails.FormTrustProposedNameOfTrust.Should().BeEquivalentTo(formTrustStateObj.FormTrustProposedNameOfTrust);
			result.TrustDetails.FormTrustReasonApprovaltoConvertasSAT.Should().Be(formTrustStateObj.FormTrustReasonApprovaltoConvertasSAT);
			result.TrustDetails.FormTrustReasonApprovedPerson.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonApprovedPerson);
			result.TrustDetails.FormTrustReasonForming.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonForming);
			result.TrustDetails.FormTrustReasonFreedom.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonFreedom);
			result.TrustDetails.FormTrustReasonGeoAreas.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonGeoAreas);
			result.TrustDetails.FormTrustReasonImproveTeaching.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonImproveTeaching);
			result.TrustDetails.FormTrustReasonVision.Should().BeEquivalentTo(formTrustStateObj.FormTrustReasonVision);
			result.TrustDetails.TrustApproverEmail.Should().BeEquivalentTo(formTrustStateObj.TrustApproverEmail);
			result.TrustDetails.TrustApproverName.Should().BeEquivalentTo(formTrustStateObj.TrustApproverName);
			result.Id.Should().Be(formTrustStateObj.Id);
		}

		[Fact]
		public void CanMap_FormTrust_MapFromDomainToServiceModel()
		{
			// Arrange
			var joinTrustDomainObj = this.fixture.Create<IJoinTrust>();

			Mock.Get(joinTrustDomainObj).Setup(x => x.Id).Returns(10101);
			Mock.Get(joinTrustDomainObj).Setup(x => x.UKPRN).Returns(295061);
			Mock.Get(joinTrustDomainObj).Setup(x => x.TrustName).Returns("Test Trust");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrust).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("ChangesToTrustExplained it has changed");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernance).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernanceExplained).Returns("ChangesToLaGovernanceExplained it has changed");

			// Act
			var result = mapper.Map<ApplicationJoinTrustServiceModel>(joinTrustDomainObj);

			// Assert
			Assert.NotNull(result);
			result.Should().BeEquivalentTo(joinTrustDomainObj);
		}
	}
}
