using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using ClosedXML.Excel;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate; 
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate; 

namespace Dfe.Academies.Academisation.SubcutaneousTest.Export
{
	public class ExportTests : ApiIntegrationTestBase
	{
		private readonly HttpClient _client;
		private readonly AcademisationContext _context;

		public ExportTests()
		{
			_client = CreateClient();
			_context = GetDBContext();
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsNotFound_WhenNoProjectsFound()
		{
			// Arrange
			var searchModel = new ConversionProjectSearchModel(1, 10, "", null!, null!, null!, null!, null!);
			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("export/export-projects", searchModel, CancellationToken);

			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), System.Net.HttpStatusCode.NotFound.GetHashCode());
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsProjectsStream_WhenProjectsFound()
		{
			// Arrange
			var expectedProject = await CreateConversionProject();
			var searchModel = new ConversionProjectSearchModel(1, 10, expectedProject.Details.NameOfTrust, null!, null!, null!, null!, null!);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("export/export-projects", searchModel, CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), System.Net.HttpStatusCode.OK.GetHashCode());

			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					httpResponseMessage.Content.Headers.ContentType!.MediaType);

			var headers = new List<string>
			{
				"School", "URN","School Type", "School Phase", "Incoming Trust", "Incoming Trust Reference Number", "Local Authority",
				"Region", "Advisory Board Date", "Decision Date", "Status", "Decision maker's name", "Decision maker's role", "Assigned To",
				"Academy Type and Route", "Part of PFI scheme", "Date AO issued", "Date dAO issued", "DAO Revoked Reasons"
			};

			VerifyWorkSheetHeaders(await httpResponseMessage.Content.ReadAsByteArrayAsync(), headers);
		}

		[Fact]
		public async Task ExportTransferProjectsToSpreadsheet_ReturnsNotFound_WhenNoProjectsFound()
		{
			// Arrange  
			var searchModel = new GetProjectSearchModel(1, 10, Fixture.Create<string>()[..10], null!, null!, null!, null!);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("export/export-transfer-projects", searchModel, CancellationToken);

			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), System.Net.HttpStatusCode.NotFound.GetHashCode());
		}

		[Fact(Skip = "Unable to mock academies API ")] 
		public async Task ExportTransferProjectsToSpreadsheet_ReturnsProjectsStream_WhenProjectsFound()
		{
			// Arrange
			var incomingTrustName = Fixture.Create<string>()[..10];
			var expectedProject = await CreateTransferProjects(incomingTrustName);
			var searchModel = new GetProjectSearchModel(1, 10, incomingTrustName, null!, null!, null!, null!);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("export/export-transfer-projects", searchModel, CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), System.Net.HttpStatusCode.OK.GetHashCode());

			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					httpResponseMessage.Content.Headers.ContentType!.MediaType);

			var headers = new List<string>
			{
				"Academy", "URN", "Academy Type", "Incoming Trust", "Incoming Trust UKPRN","Outgoing Trust", "Outgoing Trust UKPRN", "Local Authority", "Region",
				"Advisory Board Date", "Decision Date", "Status", "Decision maker's name", "Decision maker's role", "Assigned To", "Reason for transfer", "Type of transfer",
				"Proposed academy transfer date", "PFI (Private Finance Initiative)"
			};

			VerifyWorkSheetHeaders(await httpResponseMessage.Content.ReadAsByteArrayAsync(), headers);
		}
		private void VerifyWorkSheetHeaders(byte[] workSheetByteArray,  List<string> headers)
		{
			using var stream = new MemoryStream(workSheetByteArray);
			using var workbook = new XLWorkbook(stream);
			var worksheet = workbook.Worksheet("Projects");
			Assert.NotNull(worksheet);

			// Read the first column data
			for (int col = 1; col <= 19; col++)
			{
				string columnCell = worksheet.Cell(1, col).GetString();
				Assert.Equal(headers[col - 1], columnCell);
			}
		}

		private async Task<Project> CreateConversionProject()
		{
			var project = Fixture.Build<Project>()
				.Without(x => x.Id)
				.Create();

			_context.Projects.Add(project);
			await _context.SaveChangesAsync();

			await CreateConversionAdvisoryBoardDecisions(project.Id);

			return project;
		}

		private async Task CreateConversionAdvisoryBoardDecisions(int projectId, bool isTransfers = false)
		{
			var advisoryBoardDeferredReasonDetails = Fixture.Build<AdvisoryBoardDeferredReasonDetails>()
				.Create();
			var now = DateTime.UtcNow;

			var advisoryBoardDeclinedReasonDetails = new List<AdvisoryBoardWithdrawnReasonDetails>{
					new (1, AdvisoryBoardWithdrawnReason.AwaitingNextOfstedReport,"Withdrawn")
				};
			int? conversionProjectId = isTransfers ? null : projectId;
			int? transferProjectId = isTransfers ? projectId : null;

			var conversionAdvisoryBoardDecision = new ConversionAdvisoryBoardDecision(1,
				new AdvisoryBoardDecisionDetails(conversionProjectId, transferProjectId, AdvisoryBoardDecision.Withdrawn, false, "Approved", now, now,
				DecisionMadeBy.DeputyDirector, "Paull Smith"), null!, null!, advisoryBoardDeclinedReasonDetails, null, now, now);

			_context.ConversionAdvisoryBoardDecisions.Add(conversionAdvisoryBoardDecision);
			await _context.SaveChangesAsync();
		}

		private async Task<ITransferProject> CreateTransferProjects(string incomingTrustUkprn)
		{
			var outgoingTrustUkprn = "OutgoingAcademyUkprn";
			var transferAcademy = new TransferringAcademy(incomingTrustUkprn, "in trust", outgoingTrustUkprn, "region", "local authority");
			var transferringAcademies = new List<TransferringAcademy>() { transferAcademy };

			var transferProject = TransferProject.Create(outgoingTrustUkprn, "out trust", transferringAcademies, false, DateTime.Now);
			_context.TransferProjects.Add(transferProject);
			await _context.SaveChangesAsync();

			await CreateConversionAdvisoryBoardDecisions(transferProject.Id, true);

			return transferProject;
		}
	}
}
