public class IndexerService : IIndexerService
{
    private readonly IDataverseService _dataverseService;
    public IndexerService(IDataverseService dataverseService)
    {
        _dataverseService = dataverseService;
    }
    // get url from controller

    // use dataseverse service to get dataset

    // take files from dataset

    // map file to model

    // save item to db (use DAL)

    // get item(subject eg Banana) from file

    // map item to model

    // save file to db (use DAL)
}