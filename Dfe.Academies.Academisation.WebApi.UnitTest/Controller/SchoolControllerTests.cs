using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class SchoolControllerTests
	{
		private readonly Mock<ILogger<SchoolController>> _loggerMock;
		private readonly Mock<IUpdateLoanCommandHandler> _updateLoanCommandHandlerMock;
		private readonly Mock<ICreateLoanCommandHandler> _createLoanCommandHandlerMock;
		private readonly Mock<IDeleteLoanCommandHandler> _deleteLoanCommandHandlerMock;
		private readonly Mock<IUpdateLeaseCommandHandler> _updateLeaseCommandHandlerMock;
		private readonly Mock<ICreateLeaseCommandHandler> _createLeaseCommandHandlerMock;
		private readonly Mock<IDeleteLeaseCommandHandler> _deleteLeaseCommandHandlerMock;

		public SchoolControllerTests()
		{
			_loggerMock = new Mock<ILogger<SchoolController>>();
			_updateLoanCommandHandlerMock = new Mock<IUpdateLoanCommandHandler>();
			_createLoanCommandHandlerMock = new Mock<ICreateLoanCommandHandler>();
			_deleteLoanCommandHandlerMock = new Mock<IDeleteLoanCommandHandler>();
			_updateLeaseCommandHandlerMock = new Mock<IUpdateLeaseCommandHandler>();
			_createLeaseCommandHandlerMock = new Mock<ICreateLeaseCommandHandler>();
			_deleteLeaseCommandHandlerMock = new Mock<IDeleteLeaseCommandHandler>();
		}
		
		[Fact]
		public async Task UpdateLoan_ReturnsOk()
		{
			var command = new UpdateLoanCommand();
			_updateLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task UpdateLoan_ReturnsNotFound()
		{
			var command = new UpdateLoanCommand();
			_updateLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task UpdateLoan_ReturnsBadRequest()
		{
			var command = new UpdateLoanCommand();
			_updateLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLoan(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task CreateLoan_ReturnsOk()
		{
			var command = new CreateLoanCommand();
			_createLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task CreateLoan_ReturnsNotFound()
		{
			var command = new CreateLoanCommand();
			_createLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task CreateLoan_ReturnsBadRequest()
		{
			var command = new CreateLoanCommand();
			_createLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLoan(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task DeleteLoan_ReturnsOk()
		{
			var command = new DeleteLoanCommand();
			_deleteLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLoan(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task DeleteLoan_ReturnsNotFound()
		{
			var command = new DeleteLoanCommand();
			_deleteLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLoan(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task DeleteLoan_ReturnsBadRequest()
		{
			var command = new DeleteLoanCommand();
			_deleteLoanCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLoan(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task UpdateLease_ReturnsOk()
		{
			var command = new UpdateLeaseCommand();
			_updateLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task UpdateLease_ReturnsNotFound()
		{
			var command = new UpdateLeaseCommand();
			_updateLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task UpdateLease_ReturnsBadRequest()
		{
			var command = new UpdateLeaseCommand();
			_updateLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.UpdateLease(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task CreateLease_ReturnsOk()
		{
			var command = new CreateLeaseCommand();
			_createLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task CreateLease_ReturnsNotFound()
		{
			var command = new CreateLeaseCommand();
			_createLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task CreateLease_ReturnsBadRequest()
		{
			var command = new CreateLeaseCommand();
			_createLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.CreateLease(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task DeleteLease_ReturnsOk()
		{
			var command = new DeleteLeaseCommand();
			_deleteLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandSuccessResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLease(command) as OkResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
		}
		
		[Fact]
		public async Task DeleteLease_ReturnsNotFound()
		{
			var command = new DeleteLeaseCommand();
			_deleteLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new NotFoundCommandResult());

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLease(command) as NotFoundResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
		}
		
		[Fact]
		public async Task DeleteLease_ReturnsBadRequest()
		{
			var command = new DeleteLeaseCommand();
			_deleteLeaseCommandHandlerMock.Setup(x => x.Handle(command))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>{new ValidationError("Some prop", "Some error")}));

			var sut = new SchoolController(_loggerMock.Object, _createLoanCommandHandlerMock.Object,
				_updateLoanCommandHandlerMock.Object, _deleteLoanCommandHandlerMock.Object,
				_createLeaseCommandHandlerMock.Object, _updateLeaseCommandHandlerMock.Object,
				_deleteLeaseCommandHandlerMock.Object);

			var result = await sut.DeleteLease(command) as BadRequestObjectResult;
			
			Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
		}
	}
}
