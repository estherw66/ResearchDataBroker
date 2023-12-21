using ResearchDataBroker.Service;

namespace ResearchDataBroker.Tests.Controllers;

[TestFixture]
public class TestIndexController
{
    private readonly IIndexService _indexService;

    [SetUp]
    public void SetUp()
    {
        // _indexService = new IndexService();
    }
}