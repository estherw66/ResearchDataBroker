using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetFiles([FromBody]GetDatasetRequestDTO request)
    {
        // DataverseLatestVersionModel latestVersion = await _indexService
        return Ok();
    }
}