
public class IndexService : IIndexService
{
    private readonly IDataverseService _dataverseService;
    public IndexService(IDataverseService dataverseService)
    {
        _dataverseService = dataverseService;
    }

    public async Task<List<FileModel>> GetFileModels(GetDatasetRequestDTO request)
    {
        DataverseLatestVersionModel latestVersion = await _dataverseService.GetLatestVersion(request.DatasetUrl);
        List<FileModel> files = await GetFileModels(latestVersion);
        List<ItemModel> items = new List<ItemModel>();

        // check dataset type
        DatasetType type = CheckDatasetType(files);

        if (type == DatasetType.Classification)
        {
            HashSet<string> directories = GetDirectories(files);
            foreach (string dir in directories)
            {
                ItemModel item = new ItemModel
                {
                    Id = 1,
                    Name = dir
                };
                foreach (FileModel file in files)
                {
                    if (file.DirectoryLabel.Contains(dir, StringComparison.OrdinalIgnoreCase))
                    {
                        file.Items.Add(item);
                        // item.Files.Add(file);
                    }
                }
                items.Add(item);
            }
        }
        return files;
    }

    public async Task<List<ItemModel>> IndexDataset(GetDatasetRequestDTO request)
    {
        // get url from controller
        // use dataseverse service to get dataset
        // take files from dataset
        DataverseLatestVersionModel latestVersion = await _dataverseService.GetLatestVersion(request.DatasetUrl);
        List<FileModel> files = await GetFileModels(latestVersion);
        List<ItemModel> items = new List<ItemModel>();

        // check dataset type
        DatasetType type = CheckDatasetType(files);

        if (type == DatasetType.Classification)
        {
            HashSet<string> directories = GetDirectories(files);
            foreach (string dir in directories)
            {
                ItemModel item = new ItemModel
                {
                    Id = 1,
                    Name = dir
                };
                foreach (FileModel file in files)
                {
                    if (file.DirectoryLabel.Contains(dir, StringComparison.OrdinalIgnoreCase))
                    {
                        file.Items.Add(item);
                        // item.Files.Add(file);
                    }
                }
                items.Add(item);
            }
        }

        return items;
        // throw new NotImplementedException();
    }

    // map file to model
    private async Task<List<FileModel>> GetFileModels(DataverseLatestVersionModel latestVersion)
    {
        List<FileModel> fileModels = new List<FileModel>();
        foreach (DataverseFileModel fileModel in latestVersion.Files)
        {
            if (fileModel.DirectoryLabel.Contains("data", StringComparison.OrdinalIgnoreCase))
            {
                FileModel newFileModel = new FileModel
                {
                    Id = fileModel.DataFile.Id,
                    Filename = fileModel.DataFile.Filename,
                    Link = $"https://demo.dataverse.org/api/access/datafile/{fileModel.DataFile.Id}",
                    DirectoryLabel = fileModel.DirectoryLabel
                };
                fileModels.Add(newFileModel);
            }
        }

        return fileModels;
    }

    // check dataset type
    private DatasetType CheckDatasetType(List<FileModel> files)
    {
        HashSet<string> directories = GetDirectories(files);
        HashSet<string> fileExtensions = GetFileExtensions(files);

        if (directories.Count != 0)
        {
            return DatasetType.Classification;
        }

        if (fileExtensions.Contains(".xml") && fileExtensions.Contains(".jpg"))
        {
            return DatasetType.ObjectDetection;
        }

        if (fileExtensions.Contains(".csv"))
        {
            return DatasetType.Tabular;
        }

        return DatasetType.Other;
    }

    private HashSet<string> GetDirectories(List<FileModel> files)
    {
        HashSet<string> directories = new HashSet<string>();

        foreach (FileModel file in files)
        {
            string[] directoryParts = file.DirectoryLabel.Split('/');
            foreach (var part in directoryParts)
            {
                directories.Add(part.ToLower());
            }
        }

        string[] remove = new string[4] {"data", "train", "test", "eval" };

        foreach (string dir in remove)
        {
            if (directories.Contains(dir))
            {
                directories.Remove(dir);
            }
        }

        return directories;
    }

    private HashSet<string> GetFileExtensions(List<FileModel> files)
    {
        HashSet<string> fileExtensions = new HashSet<string>();

        foreach (FileModel file in files)
        {
            if (file.DirectoryLabel.Contains("data", StringComparison.OrdinalIgnoreCase))
            {
                FileInfo fi = new FileInfo(file.Filename);
                string ext = fi.Extension;
                fileExtensions.Add(ext);
            }
        }

        foreach (string ext in fileExtensions)
        {
            Console.WriteLine(ext);
        }

        return fileExtensions;
    }

    

    // save item to db (use DAL)

    // get item(subject eg Banana) from file

    // map item to model

    // save file to db (use DAL)
}