using System;
using Microsoft.AspNetCore.Mvc;

namespace ResearchDataBroker.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			Dictionary<string, string> message = new Dictionary<string, string>
			{
				{ "message", "Test message" }
			};

			return Ok(message);
        }
	}
}

