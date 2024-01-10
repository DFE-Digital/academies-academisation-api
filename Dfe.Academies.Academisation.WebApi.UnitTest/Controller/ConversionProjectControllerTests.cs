using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ConversionProjectControllerTests
	{
		private readonly Mock<IConversionProjectQueryService> _mockConversionProjectQueryService;
		private readonly Mock<IMediator> _mockMediator;
		private readonly ConversionProjectController _controller;

		public ConversionProjectControllerTests()
		{
			_mockConversionProjectQueryService = new Mock<IConversionProjectQueryService>();
			_mockMediator = new Mock<IMediator>();
			_controller = new ConversionProjectController(_mockConversionProjectQueryService.Object, _mockMediator.Object);
		}

		private SetSchoolOverviewCommand CreateValidSetSchoolOverviewCommand()
		{
			return new SetSchoolOverviewCommand(
				id: 1,
				publishedAdmissionNumber: "100",
				viabilityIssues: "No issues",
				partOfPfiScheme: "No",
				financialDeficit: "None",
				numberOfPlacesFundedFor: 150m,
				numberOfResidentialPlaces: 10m,
				numberOfFundedResidentialPlaces: 5m,
				pfiSchemeDetails: "N/A",
				distanceFromSchoolToTrustHeadquarters: 20m,
				distanceFromSchoolToTrustHeadquartersAdditionalInformation: "Within city limits",
				memberOfParliamentNameAndParty: "Jane Doe - PartyName",
				pupilsAttendingGroupPermanentlyExcluded: true,
				pupilsAttendingGroupMedicalAndHealthNeeds: true,
				pupilsAttendingGroupTeenageMums: true
			);
		}

		[Fact]
		public async Task SetSchoolOverview_ReturnsOk_WhenUpdateIsSuccessful()
		{
			var request = CreateValidSetSchoolOverviewCommand();
			_mockMediator.Setup(m => m.Send(It.IsAny<SetSchoolOverviewCommand>(), default))
						 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetSchoolOverview(request.Id, request);

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetSchoolOverview_ReturnsNotFound_WhenProjectNotFound()
		{
			var request = CreateValidSetSchoolOverviewCommand();
			_mockMediator.Setup(m => m.Send(It.IsAny<SetSchoolOverviewCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetSchoolOverview(request.Id, request);

			Assert.IsType<NotFoundResult>(result);
		}


	}
}
