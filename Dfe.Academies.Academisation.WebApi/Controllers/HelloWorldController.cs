using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase
{
	private readonly ILogger<HelloWorldController> _logger;
	private readonly HelloWorldOptions _helloWorldOptions;

	public HelloWorldController(ILogger<HelloWorldController> logger, IOptions<HelloWorldOptions> helloWorldOptions)
	{
		_logger = logger;
		_helloWorldOptions = helloWorldOptions.Value;
	}

	[HttpGet]
	public string Get()
	{
		return $"hello {_helloWorldOptions.Greeting}";
	}
}