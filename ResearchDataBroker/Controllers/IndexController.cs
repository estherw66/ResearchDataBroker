using Microsoft.AspNetCore.Mvc;

namespace ResearchDataBroker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndexController : ControllerBase
{
    private readonly IIndexService _indexService;

    public IndexController(IIndexService indexService)
    {
        _indexService = indexService;
    }
    // index files
    // enter url of dataset
    [HttpPost]
    public async Task<IActionResult> IndexDataset([FromBody]GetDatasetLatestVersionRequestDTO latestVersionRequest)
    {
        return Ok(await _indexService.IndexDataset(latestVersionRequest));
    }

    [HttpGet("files")]
    public async Task<IActionResult> GetFiles()
    {
        return Ok(await _indexService.GetFiles());
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        return Ok(await _indexService.GetItems());
    }
}