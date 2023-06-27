using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class TestController : ControllerBase
	{
		private readonly IMaintainTestProjectCommand _maintainTestProjectCommand;
		public TestController(IMaintainTestProjectCommand maintainTestProjectCommand)
		{
			_maintainTestProjectCommand = maintainTestProjectCommand;
		}
		
		/// <summary>
		/// Returns the test project.
		/// </summary>
		/// <returns><see cref="TestProjectModel"/></returns>
		/// <response code="200">The test project was returned</response>
		[HttpGet]
		[Route("test/maintainTestProject")]
		public async Task<ActionResult<TestProjectModel>> MaintainTestProject()
		{
			var result = await _maintainTestProjectCommand.Execute();
			return result switch
			{
				CreateSuccessResult<TestProjectModel> => Ok(result),
				_ => throw new NotImplementedException()
			};

		}
	}

	
}
