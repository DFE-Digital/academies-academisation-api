using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Summary;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class SummaryQueryService(
		IConversionProjectRepository conversionProjectRepository,
		ITransferProjectRepository transferProjectRepository,
		IFormAMatProjectRepository formAMatProjectRepository) : ISummaryQueryService
	{
		public async Task<IEnumerable<ProjectSummary>> GetProjectSummariesByAssignedEmail(string email,
			bool includeConversions, bool includeTransfers, bool includeFormAMat, CancellationToken cancellationToken)
		{
			List<ProjectSummary> conversionQuery = [];
			List<ProjectSummary> transferQuery = [];
			List<ProjectSummary> formAMatQuery = [];

			if (includeConversions)
			{
				conversionQuery = await GetConversionQuery(email, cancellationToken);
			}

			if (includeTransfers)
			{
				transferQuery = await GetTransferQuery(email, cancellationToken);
			}

			if (includeFormAMat)
			{
				formAMatQuery = await GetFormAMatQuery(email, cancellationToken);
			}

			var projectSummaries = conversionQuery
				.Concat(transferQuery)
				.Concat(formAMatQuery);

			return projectSummaries;
		}

		private async Task<List<ProjectSummary>> GetFormAMatQuery(string email, CancellationToken cancellationToken)
		{
			var formAMats = (await formAMatProjectRepository.GetByEmail(email, cancellationToken)).ToList();

			var projects = (await conversionProjectRepository.GetConversionProjectsByFormAMatIds(
				formAMats.Select(x => x.Id as int?),
				cancellationToken)).ToList();

			return formAMats.ToList().Select(formAMat => new ProjectSummary()
			{
				Id = formAMat.Id,
				CreatedOn = formAMat.CreatedOn,
				LastModifiedOn = formAMat.LastModifiedOn,
				ConversionsSummary = null,
				FormAMatSummary = new FormAMatSummary()
				{
					ProposedTrustName = formAMat.ProposedTrustName,
					SchoolNames =
						projects.Where(x => x.FormAMatProjectId == formAMat.Id && x.Details.SchoolName != null)
							.Select(x => x.Details.SchoolName!).ToArray(),
					LocalAuthority =
						projects.Where(x => x.FormAMatProjectId == formAMat.Id && x.Details.SchoolName != null)
							.Select(x => x.Details.LocalAuthority!).ToArray(),
					AdvisoryBoardDate = projects.Where(x => x.FormAMatProjectId == formAMat.Id)
						.Select(x => x.Details.HeadTeacherBoardDate).OrderDescending().FirstOrDefault(),
				}
			}).ToList();
		}

		private async Task<List<ProjectSummary>> GetTransferQuery(string email, CancellationToken cancellationToken)
		{
			var projects = await transferProjectRepository.GetByDeliveryOfficerEmail(email, cancellationToken);

			return projects.Select(x => new ProjectSummary
			{
				Id = x.Id,
				CreatedOn = x.CreatedOn,
				LastModifiedOn = x.LastModifiedOn,
				TransfersSummary = new TransfersSummary
				{
					Urn = x.Urn,
					ProjectReference = x.ProjectReference,
					OutgoingTrustUkprn = x.OutgoingTrustUkprn,
					OutgoingTrustName = x.OutgoingTrustName,
					TypeOfTransfer = x.TypeOfTransfer,
					TargetDateForTransfer = x.TargetDateForTransfer,
					AssignedUserEmailAddress = x.AssignedUserEmailAddress,
					AssignedUserFullName = x.AssignedUserFullName,
					Status = x.Status,
					IncomingTrustName = x.TransferringAcademies.Select(x => x.IncomingTrustName).First()
				}
			}).ToList();
		}

		private async Task<List<ProjectSummary>> GetConversionQuery(string email, CancellationToken cancellationToken)
		{
			var projects = await conversionProjectRepository.GetConversionProjectsByEmail(email, cancellationToken);
			return projects.Select(x =>
				new ProjectSummary
				{
					Id = x.Id,
					CreatedOn = x.CreatedOn,
					LastModifiedOn = x.LastModifiedOn,
					ConversionsSummary = new ConversionsSummary
					{
						Urn = x.Details.Urn,
						ApplicationReferenceNumber = x.Details.ApplicationReferenceNumber,
						SchoolName = x.Details.SchoolName,
						LocalAuthority = x.Details.LocalAuthority,
						Region = x.Details.Region,
						AcademyTypeAndRoute = x.Details.AcademyTypeAndRoute,
						NameOfTrust = x.Details.NameOfTrust,
						AssignedUserEmailAddress =
							x.Details.AssignedUser?.EmailAddress,
						AssignedUserFullName =
							x.Details.AssignedUser?.FullName,
						ProjectStatus = x.Details.ProjectStatus,
						TrustReferenceNumber = x.Details.TrustReferenceNumber,
					}
				}).ToList();



			//return _context.Projects
			//	.Where(x =>
			//		(x.Details.ProjectStatus == "Converter Pre-AO (C)" || x.Details.ProjectStatus == "Deferred") &&
			//		x.Details.AssignedUser != null &&
			//		x.Details.AssignedUser.EmailAddress == email
			//	)
			//	.Select(x => new ProjectSummaryIntermediate
			//	{
			//		Id = x.Id,
			//		Urn = x.Details.Urn,
			//		CreatedOn = x.CreatedOn,
			//		LastModifiedOn = x.LastModifiedOn,
			//		ConversionsSummary = new ConversionsSummary
			//		{
			//			ApplicationReferenceNumber = x.Details.ApplicationReferenceNumber,
			//			SchoolName = x.Details.SchoolName,
			//			LocalAuthority = x.Details.LocalAuthority,
			//			Region = x.Details.Region,
			//			AcademyTypeAndRoute = x.Details.AcademyTypeAndRoute,
			//			NameOfTrust = x.Details.NameOfTrust,
			//			AssignedUserEmailAddress = x.Details.AssignedUser != null ? x.Details.AssignedUser.EmailAddress : null,
			//			AssignedUserFullName = x.Details.AssignedUser != null ? x.Details.AssignedUser.FullName : null,
			//			ProjectStatus = x.Details.ProjectStatus,
			//			TrustReferenceNumber = x.Details.TrustReferenceNumber,
			//		},
			//		TransfersSummary = null
			//	});
		}
	}
}
