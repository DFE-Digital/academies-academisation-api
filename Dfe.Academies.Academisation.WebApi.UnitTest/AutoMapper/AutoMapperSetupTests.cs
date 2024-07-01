using System;
using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using FluentAssertions;
using Moq;
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

			mapper = new Mapper(mapperConfig);
		}



		[Fact]
		public void CanMap_JoinTrust_MapFromDomainToServiceModel()
		{
			// Arrange
			var joinTrustDomainObj = this.fixture.Create<IJoinTrust>();

			Mock.Get(joinTrustDomainObj).Setup(x => x.Id).Returns(10101);
			Mock.Get(joinTrustDomainObj).Setup(x => x.UKPRN).Returns(295061);
			Mock.Get(joinTrustDomainObj).Setup(x => x.TrustName).Returns("Test Trust");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrust).Returns(ChangesToTrust.Yes);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToTrustExplained).Returns("ChangesToTrustExplained it has changed");
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernance).Returns(true);
			Mock.Get(joinTrustDomainObj).Setup(x => x.ChangesToLaGovernanceExplained).Returns("ChangesToLaGovernanceExplained it has changed");
			Mock.Get(joinTrustDomainObj).Setup(x => x.TrustReference).Returns("TR0001");
			// Act
			var result = mapper.Map<ApplicationJoinTrustServiceModel>(joinTrustDomainObj);

			// Assert
			Assert.NotNull(result);

			result.Should().BeEquivalentTo(joinTrustDomainObj);
		}

		[Fact]
		public void CanMap_FormTrust_MapFromDomainToServiceModel()
		{
			// Arrange
			IFormTrust formTrustDomainObj = this.fixture.Create<IFormTrust>();

			// relying on the all details been set here by autofixture
			var trustDetails = this.fixture.Create<FormTrustDetails>();
			var keyPerson = this.fixture.Create<ITrustKeyPerson>();
			Mock.Get(keyPerson).Setup(x => x.Id).Returns(this.fixture.Create<int>());
			Mock.Get(keyPerson).Setup(x => x.Name).Returns(this.fixture.Create<string>());
			Mock.Get(keyPerson).Setup(x => x.DateOfBirth).Returns(this.fixture.Create<DateTime>());
			Mock.Get(keyPerson).Setup(x => x.Biography).Returns(this.fixture.Create<string>());
			Mock.Get(keyPerson).Setup(x => x.Roles).Returns(new List<ITrustKeyPersonRole> { TrustKeyPersonRole.Create(KeyPersonRole.Trustee, "10 months") }.AsReadOnly());

			Mock.Get(formTrustDomainObj).Setup(x => x.Id).Returns(10101);
			Mock.Get(formTrustDomainObj).Setup(x => x.TrustDetails).Returns(trustDetails);
			Mock.Get(formTrustDomainObj).Setup(x => x.KeyPeople).Returns(new List<ITrustKeyPerson> { keyPerson }.AsReadOnly());

			// Act
			var result = mapper.Map<ApplicationFormTrustServiceModel>(formTrustDomainObj);

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
			result.KeyPeople.Count.Should().Be(1);
			result.KeyPeople[0].Id.Should().Be(keyPerson.Id);
			result.KeyPeople[0].Biography.Should().Be(keyPerson.Biography);
			result.KeyPeople[0].DateOfBirth.Should().Be(keyPerson.DateOfBirth);
			result.KeyPeople[0].Name.Should().Be(keyPerson.Name);
			result.Id.Should().Be(formTrustDomainObj.Id);
		}
	}
}
