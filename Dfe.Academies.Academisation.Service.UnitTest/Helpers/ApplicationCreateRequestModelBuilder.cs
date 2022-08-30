using Bogus;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.UnitTest.Helpers
{
	internal class ApplicationCreateRequestModelBuilder
	{
		private ApplicationType _applicationType = ApplicationType.JoinAMat;
		private ContributorRequestModel _contributorRequestModel = new ContributorRequestModelBuilder().Build();

		private static readonly Faker _faker = new();


		public ApplicationCreateRequestModel Build()
		{
			return new ApplicationCreateRequestModel(_applicationType, _contributorRequestModel);
		}

		public ApplicationCreateRequestModelBuilder WithApplicationType(ApplicationType applicationType)
		{
			_applicationType = applicationType;
			return this;
		}

		public ApplicationCreateRequestModelBuilder WithContributorDetails(ContributorRequestModel contributorDetailsRequestModel)
		{
			_contributorRequestModel = contributorDetailsRequestModel;
			return this;
		}
	}

	internal class ContributorRequestModelBuilder
	{
		private static readonly Faker _faker = new();

		private readonly string _firstName = _faker.Name.FirstName();
		private readonly string _lastName = _faker.Name.LastName();
		private readonly string _email = _faker.Internet.Email();
		private ContributorRole _contributorRole = ContributorRole.ChairOfGovernors;
		private string? _role = null;

		public ContributorRequestModel Build()
		{
			return new(_firstName, _lastName, _email, _contributorRole, _role);
		}

		public ContributorRequestModelBuilder WithContributorRole(ContributorRole contributorRole)
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
