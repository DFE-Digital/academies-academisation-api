using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Service.Commands;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class ApplicationSubmitCommandTests
{
	private readonly Fixture fixture = new();

	private readonly Mock<IApplicationGetDataQuery> _getDataQueryMock = new();
	private readonly Mock<IApplicationUpdateDataCommand> _updateDataCommandMock = new();
	private readonly Mock<IConversionApplication> _conversionApplicationMock = new();
	private readonly int _applicationId;

	private readonly ApplicationSubmitCommand _subject;

	public ApplicationSubmitCommandTests()
	{
		_applicationId = fixture.Create<int>();
		_subject = new(_getDataQueryMock.Object, _updateDataCommandMock.Object);
	}

	[Fact]
	public async Task SubmitSuccessful___PassedToDataLayer_SuccessReturned()
	{
		// arrange
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync(_conversionApplicationMock.Object);
		_conversionApplicationMock.Setup(x => x.Submit()).Returns(new CommandSuccessResult());
		
		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		Assert.IsType<CommandSuccessResult>(result);
		_conversionApplicationMock.Verify(x => x.Submit(), Times.Once());
		_updateDataCommandMock.Verify(x => x.Execute(_conversionApplicationMock.Object), Times.Once);
	}

	[Fact]
	public async Task SubmitUnsuccessful___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync(_conversionApplicationMock.Object);

		CommandValidationErrorResult commandValidationErrorResult = new(new List<ValidationError>());
		_conversionApplicationMock.Setup(x => x.Submit()).Returns(commandValidationErrorResult);

		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		Assert.IsType<CommandValidationErrorResult>(result);
		Assert.Equal(commandValidationErrorResult, result);
		_conversionApplicationMock.Verify(x => x.Submit(), Times.Once());
		_updateDataCommandMock.Verify(x => x.Execute(_conversionApplicationMock.Object), Times.Never);
	}
}
