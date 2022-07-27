using Bogus;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.UnitTest.Helpers
{
	internal class ApplicationCreateRequestModelBuilder
	{
		private ApplicationType _applicationType = ApplicationType.JoinAMat;
		private ContributorDetailsRequestModel _contributorDetailsRequestModel = new ContributorDetailsRequestModelBuilder().Build();

		private static Faker _faker = new();


		public ApplicationCreateRequestModel Build()
		{
			return new ApplicationCreateRequestModel(_applicationType, _contributorDetailsRequestModel);
		}

		public ApplicationCreateRequestModelBuilder WithApplicationType(ApplicationType applicationType)
		{
			_applicationType = applicationType;
			return this;
		}

		public ApplicationCreateRequestModelBuilder WithContributorDetails(ContributorDetailsRequestModel contributorDetailsRequestModel)
		{
			_contributorDetailsRequestModel = contributorDetailsRequestModel;
			return this;
		}
	}

	internal class ContributorDetailsRequestModelBuilder
	{
		private static readonly Faker _faker = new Faker();

		private string _firstName = _faker.Name.FirstName();
		private string _lastName = _faker.Name.LastName();
		private string _email = _faker.Internet.Email();
		private ContributorRole _contributorRole = ContributorRole.ChairOfGovernors;
		private string? _role = null;

		public ContributorDetailsRequestModel Build()
		{
			return new(_firstName, _lastName, _email, _contributorRole, _role);
		}

		public ContributorDetailsRequestModelBuilder WithContributorRole(ContributorRole contributorRole)
		{
			_contributorRole = contributorRole;
			if (contributorRole == ContributorRole.Other)
			{
				_role = "Other";
			}
			return this;
		}
	}
}
