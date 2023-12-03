using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ResearchDataBroker.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestController : ControllerBase
	{
		private readonly IDataverseService _dataverseService;

		public TestController(IDataverseService dataverseService)
		{
			_dataverseService = dataverseService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			Dictionary<string, string> message = new Dictionary<string, string>
			{
				{ "message", "Test message" }
			};

			return Ok(message);
        }

		[HttpGet("url")]
		public async Task<IActionResult> GetResponse([FromBody]GetDatasetRequestDTO request)
		{
			DataverseLatestVersionModel response = await _dataverseService.GetLatestVersion(request);

			if (response == null)
			{
				return BadRequest("no response");
			}

			return Ok(response);
		}

	}
}

