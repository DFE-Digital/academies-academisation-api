using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Summary;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class SummaryQueryService(AcademisationContext context) : ISummaryQueryService
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));
		
        public Task<IEnumerable<ProjectSummary>> GetProjectSummariesByAssignedEmail(string email, bool includeConversions, bool includeTransfers, bool includeFormAMat)
        {
            IEnumerable<ProjectSummaryIntermediate> conversionQuery = [];
            IEnumerable<ProjectSummaryIntermediate> transferQuery = [];

            if (includeConversions)
            {
                conversionQuery = GetConversionQuery(email);
            }

            if (includeTransfers)
            {
                transferQuery = GetTransferQuery(email);
            }

            var projectSummaries = conversionQuery
                .Concat(transferQuery)
                .Select(x => new ProjectSummary
                {
                    Id = x.Id,
                    Urn = x.Urn,
                    CreatedOn = x.CreatedOn,
                    LastModifiedOn = x.LastModifiedOn,
                    ConversionsSummary = x.ConversionsSummary,
                    TransfersSummary = x.TransfersSummary
                });

            return Task.FromResult(projectSummaries);
        }

        private IQueryable<ProjectSummaryIntermediate> GetTransferQuery(string email)
        {
	        return _context.TransferProjects
		        .Where(x => x.AssignedUserEmailAddress == email)
		        .Select(x => new
		        {
			        x.Id,
			        x.Urn,
			        x.CreatedOn,
			        x.LastModifiedOn,
			        x.ProjectReference,
			        x.OutgoingTrustUkprn,
			        x.OutgoingTrustName,
			        x.TypeOfTransfer,
			        x.TargetDateForTransfer,
			        x.AssignedUserEmailAddress,
			        x.AssignedUserFullName,
			        x.Status,
			        FirstAcademy = x.TransferringAcademies.FirstOrDefault()
		        })
		        .Select(x => new ProjectSummaryIntermediate
		        {
			        Id = x.Id,
			        Urn = x.Urn,
			        CreatedOn = x.CreatedOn,
			        LastModifiedOn = x.LastModifiedOn,
			        ConversionsSummary = null,
			        TransfersSummary = new TransfersSummary
			        {
				        ProjectReference = x.ProjectReference,
				        OutgoingTrustUkprn = x.OutgoingTrustUkprn,
				        OutgoingTrustName = x.OutgoingTrustName,
				        TypeOfTransfer = x.TypeOfTransfer,
				        TargetDateForTransfer = x.TargetDateForTransfer,
				        AssignedUserEmailAddress = x.AssignedUserEmailAddress,
				        AssignedUserFullName = x.AssignedUserFullName,
				        Status = x.Status,
				        IncomingTrustUkprn = x.FirstAcademy != null ? x.FirstAcademy.IncomingTrustUkprn : null,
				        IncomingTrustName = x.FirstAcademy != null ? x.FirstAcademy.IncomingTrustName : null
			        }
		        });
        }

        private IQueryable<ProjectSummaryIntermediate> GetConversionQuery(string email)
        {
	        return _context.Projects
		        .Where(x =>
			        (x.Details.ProjectStatus == "Converter Pre-AO (C)" || x.Details.ProjectStatus == "Deferred") &&
			        x.Details.AssignedUser != null &&
			        x.Details.AssignedUser.EmailAddress == email
		        )
		        .Select(x => new ProjectSummaryIntermediate
		        {
			        Id = x.Id,
			        Urn = x.Details.Urn,
			        CreatedOn = x.CreatedOn,
			        LastModifiedOn = x.LastModifiedOn,
			        ConversionsSummary = new ConversionsSummary
			        {
				        ApplicationReferenceNumber = x.Details.ApplicationReferenceNumber,
				        SchoolName = x.Details.SchoolName,
				        LocalAuthority = x.Details.LocalAuthority,
				        Region = x.Details.Region,
				        AcademyTypeAndRoute = x.Details.AcademyTypeAndRoute,
				        NameOfTrust = x.Details.NameOfTrust,
				        AssignedUserEmailAddress = x.Details.AssignedUser != null ? x.Details.AssignedUser.EmailAddress : null,
				        AssignedUserFullName = x.Details.AssignedUser != null ? x.Details.AssignedUser.FullName : null,
				        ProjectStatus = x.Details.ProjectStatus,
				        TrustReferenceNumber = x.Details.TrustReferenceNumber,
			        },
			        TransfersSummary = null
		        });
        }
	}
	
	public class ProjectSummaryIntermediate
	{
		public int Id { get; set; }
		public int Urn { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? LastModifiedOn { get; set; }
		public ConversionsSummary? ConversionsSummary { get; set; }
		public TransfersSummary? TransfersSummary { get; set; }
	}
}
