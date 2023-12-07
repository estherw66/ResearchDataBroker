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
		private readonly IIndexService _indexService;

		public TestController(IDataverseService dataverseService, IIndexService indexService)
		{
			_dataverseService = dataverseService;
			_indexService = indexService;
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
			DataverseLatestVersionModel response = await _dataverseService.GetLatestVersion(request.DatasetUrl);

			if (response == null)
			{
				return BadRequest("no response");
			}

			return Ok(response);
		}

		[HttpGet("files")]
		public async Task<IActionResult> GetFiles([FromBody]GetDatasetRequestDTO request)
		{
			List<FileModel> response = await _indexService.GetFileModels(request);
			// List<ItemModel> response = await _indexService.IndexDataset(request);

			return Ok(response);
		}
	}
}

