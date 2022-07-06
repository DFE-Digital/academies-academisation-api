using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HelloWorldController : ControllerBase
	{
		private readonly ILogger<HelloWorldController> _logger;

		public HelloWorldController(ILogger<HelloWorldController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new List<string> { "hello", "world" };
		}
	}
}
