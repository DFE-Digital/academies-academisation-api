using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class SchoolControllerTests
	{
		private readonly Mock<ILogger<SchoolController>> _loggerMock;
		private readonly Mock<IMediator> _mediatrMock;
		public SchoolControllerTests()
		{
			_mediatrMock = new Mock<IMediator>();
			_loggerMock = new Mock<ILogger<SchoolController>>();
		}
		
		[Fact]
		public async Task UpdateLoan_ReturnsOk()
		{
			var command = new UpdateLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.UpdateLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task UpdateLoan_ReturnsNotFound()
		{
			var command = new UpdateLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.UpdateLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task CreateLoan_ReturnsOk()
		{
			var command = new CreateLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object,_mediatrMock.Object);

			var result = await sut.CreateLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task CreateLoan_ReturnsNotFound()
		{
			var command = new CreateLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.CreateLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task DeleteLoan_ReturnsOk()
		{
			var command = new DeleteLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.DeleteLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task DeleteLoan_ReturnsNotFound()
		{
			var command = new DeleteLoanCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.DeleteLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task UpdateLease_ReturnsOk()
		{
			var command = new UpdateLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.UpdateLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task UpdateLease_ReturnsNotFound()
		{
			var command = new UpdateLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.UpdateLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task CreateLease_ReturnsOk()
		{
			var command = new CreateLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.CreateLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task CreateLease_ReturnsNotFound()
		{
			var command = new CreateLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.CreateLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task DeleteLease_ReturnsOk()
		{
			var command = new DeleteLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());
			
			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.DeleteLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task DeleteLease_ReturnsNotFound()
		{
			var command = new DeleteLeaseCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.DeleteLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task SetAdditionalDetails_ReturnsOk()
		{
			var command = new SetAdditionalDetailsCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());
			
			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.SetAdditionalDetails(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task SetAdditionalDetails_ReturnsNotFound()
		{
			var command = new SetAdditionalDetailsCommand();
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _mediatrMock.Object);

			var result = await sut.SetAdditionalDetails(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
	}
}
