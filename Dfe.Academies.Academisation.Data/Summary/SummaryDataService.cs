using Dfe.Academies.Academisation.Domain.Summary;

namespace Dfe.Academies.Academisation.Data.Summary
{
	public class SummaryDataService(AcademisationContext context) : ISummaryDataService
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<ProjectSummary>> GetProjectSummariesByAssignedEmail(string email, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm)
        {
            IEnumerable<ProjectSummaryIntermediate> conversionQuery = [];
            IEnumerable<ProjectSummaryIntermediate> transferQuery = [];

            if (includeConversions)
            {
				conversionQuery = _context.Projects
	                .Where(x => x.Details.ProjectStatus == "Converter Pre-AO (C)" || x.Details.ProjectStatus == "Deferred")
                    .Where(x => x.Details.AssignedUser.EmailAddress == email)
                    .Where(x => searchTerm == null || x.Details.Urn.ToString().Contains(searchTerm) || x.Details.SchoolName.ToString().Contains(searchTerm))
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

            if (includeTransfers)
            {
                transferQuery = _context.TransferProjects
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
                    })
                    .Where(x => searchTerm == null || x.Urn.ToString().Contains(searchTerm) || x.TransfersSummary.IncomingTrustName.Contains(searchTerm)) ;
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
                })
                .ToList();

            return projectSummaries;
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
