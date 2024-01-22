using Microsoft.AspNetCore.Mvc;
using ResearchDataBroker.DTO;

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

    [HttpGet("files/{item}")]
    public async Task<IActionResult> GetFilesByItem(string item)
    {
        return Ok(await _indexService.GetFilesByItem(item));
    }

    // [HttpGet("item/{filename}")]
    // public OkObjectResult GetItemByFile(string filename)
    // {
    //     return Ok(_indexService.GetItemByFilename(filename));
    // }
}