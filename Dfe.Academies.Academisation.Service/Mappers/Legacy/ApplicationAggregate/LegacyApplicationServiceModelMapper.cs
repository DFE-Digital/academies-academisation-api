using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ApplicationAggregate;

internal class LegacyApplicationServiceModelMapper
{
	internal static LegacyApplicationServiceModel MapToLegacyServiceModel(IApplication application)
	{
		LegacyApplicationServiceModel serviceModel = new(
			LegacyKeyPersonServiceModelMapper.MapToServiceModels(application),
			LegacySchoolServiceModelMapper.MapToServiceModels(application)
			)
		{
			ApplicationId = application.ApplicationId.ToString(),

			// For now this legacy service model only supports JoinAMat
			ApplicationType = MapType(application),
			ApplicationSubmitted = application.ApplicationStatus == ApplicationStatus.Submitted,

			// This doesn;t seem to be set in the Azure Data Factory pipeline, so leaving it null
			ApplicationStatusId = null,

			// ToDo: ApplicationReference - we need to generate an application reference somehow
			////Name = application.ApplicationReference,
		};

		// How do we determine the Lead Author, we have a list of contributors?
		// Is it just the person that created the Application?
		IContributor? leadAuthor = application.Contributors.FirstOrDefault();

		if (leadAuthor is not null)
		{
			serviceModel = serviceModel with
			{
				ApplicationLeadAuthorId = leadAuthor.Id.ToString(),
				ApplicationLeadEmail = leadAuthor.Details.EmailAddress,
				ApplicationVersion = null,
				ApplicationLeadAuthorName = $"{leadAuthor.Details.FirstName} {leadAuthor.Details.LastName}",
				ApplicationRole = null,
				ApplicationRoleOtherDescription = null,
			};
		}

		if (application.ApplicationType == ApplicationType.JoinAMat)
		{
			serviceModel = serviceModel with
			{
				// I think these will come from application.ExistingTrust but we don't have that yet
				TrustName = null,
				TrustId = null,
				ChangesToTrust = null,
				ChangesToTrustExplained = null,
				ChangesToLaGovernance = null,
				ChangesToLaGovernanceExplained = null,
				TrustApproverName = null,
				TrustApproverEmail = null,
			};
		}
		else
		{
			// For FormAMat and FormASat it would be necessary to set these fields
			// (but it's probably not worth implementing them in this legacy service model).
			// If we do, then these fields will likely come from application.NewTrust
			serviceModel = serviceModel with
			{
				FormTrustProposedNameOfTrust = null,
				FormTrustOpeningDate = null,
				FormTrustReasonApprovalToConvertAsSat = null,
				FormTrustReasonApprovedPerson = null,
				FormTrustReasonForming = null,
				FormTrustReasonVision = null,
				FormTrustReasonGeoAreas = null,
				FormTrustReasonFreedom = null,
				FormTrustReasonImproveTeaching = null,
				FormTrustPlanForGrowth = null,
				FormTrustPlansForNoGrowth = null,
				FormTrustGrowthPlansYesNo = null,
				FormTrustImprovementSupport = null,
				FormTrustImprovementStrategy = null,
				FormTrustImprovementApprovedSponsor = null,
			};

			throw new NotImplementedException("Only JoinAMat has been implemented");
		}

		return serviceModel;
	}

	private static string MapType(IApplication application)
	{
		return application.ApplicationType switch
		{
			ApplicationType.JoinAMat => "JoinMat",
			ApplicationType.FormAMat => "FormMat",
			ApplicationType.FormASat => "FormSat",
			_ => throw new NotImplementedException("Unrecognised ApplicationType, has a new ApplicationType been added recently?"),
		};
	}
}
