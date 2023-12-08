
public class IndexService : IIndexService
{
    private readonly IDataverseService _dataverseService;
    private readonly IItemRepository _itemRepository;
    private readonly IFilesRepository _filesRepository;
    public IndexService(IDataverseService dataverseService, IItemRepository itemRepository, IFilesRepository filesRepository)
    {
        _dataverseService = dataverseService;
        _itemRepository = itemRepository;
        _filesRepository = filesRepository;
    }

    public async Task<GetFilesResponseDTO> GetFiles()
    {
        List<FileDTO> fileDTOs = GetAllFiles()
            .Select(FileDTOConverter.ConvertToDTO)
            .ToList();

        return new GetFilesResponseDTO
        {
            Files = fileDTOs
        };
    }

    public async Task<IndexDatasetResponseDTO> IndexDataset(GetDatasetRequestDTO request)
    {
        // 1 get dataset from dataverse
        DataverseLatestVersionModel latestVersion = await _dataverseService.GetLatestVersion(request.DatasetUrl);

        // 2 map dataset to file models
        List<FileModel> files = await GetFileModels(latestVersion);

        // 3 (for now only classification works)
        DatasetType type = CheckDatasetType(files);
        if (type != DatasetType.Classification)
        {
            return null;
        }
        // save each file in dataset and each item belonging to file to db
        List<ItemDTO> items = new List<ItemDTO>();
        foreach (FileModel file in files)
        {
            FileModel savedFile = SaveNewFile(file);
            ItemModel savedItem = GetItem(file);

            // .Select(FileDTOConverter.ConvertToDTO)
            // .ToList();
            

            // link files and items
            if (!savedFile.Items.Any(item => item.Name == savedItem.Name))
            {
                savedFile.Items.Add(savedItem);
            }
            if (!savedItem.Files.Any(file => file.Id == savedFile.Id))
            {
                savedItem.Files.Add(savedFile);
            }
            ItemDTO itemDTO = ItemDTOConverter.ConvertToDTO(savedItem);
            items.Add(itemDTO);
        }
        // return list of indexed items

        // ItemDTO item = new ItemDTO
        // {
        //     Id = 1,
        //     Name = "item name"
        // };
        // items.Add(item);
        return new IndexDatasetResponseDTO
        {
            ItemDTOs = items
        };
        // 4
        // DatasetType type = CheckDatasetType(files);

        // 5
        // if (type == DatasetType.Classification)
        // {
        //     HashSet<string> directories = GetDirectories(files);
        //     HashSet<ItemModel> itemModels = GetItemsClassification(directories);

        //     // foreach (ItemModel i in itemModels)
        //     // {
        //     //     ItemModel savedItem = SaveItem(i);
        //     // }
        // }

        // 6
        // if (type != DatasetType.Classification)
        // {

        // }

        // 7


        // if (type == DatasetType.Classification)
        // {
        //     HashSet<string> directories = GetDirectories(files);
        //     foreach (string dir in directories)
        //     {
        //         ItemModel item = new ItemModel
        //         {
        //             Name = dir
        //         };
        //         item = _itemRepository.SaveItem(item);
        //         foreach (FileModel file in files)
        //         {
        //             if (file.DirectoryLabel.Contains(dir, StringComparison.OrdinalIgnoreCase))
        //             {
        //                 file.Items.Add(item);
        //                 // item.Files.Add(file);
        //             }
        //         }
        //         // items.Add(item);
        //     }
        // }


        // throw new NotImplementedException();
    }

    // 2
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

    // 3
    // save files to db
    private FileModel SaveNewFile(FileModel file)
    {
        return _filesRepository.SaveFile(file);
    }

    // 4
    // get directories
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

        string[] remove = new string[4] { "data", "train", "test", "eval" };

        foreach (string dir in remove)
        {
            if (directories.Contains(dir))
            {
                directories.Remove(dir);
            }
        }

        return directories;
    }
    private string GetDirectory(FileModel file)
    {
        HashSet<string> directories = new HashSet<string>();

        string[] directoryParts = file.DirectoryLabel.Split('/');
        foreach (var part in directoryParts)
        {
            directories.Add(part.ToLower());
        }

        string[] remove = new string[4] { "data", "train", "test", "eval" };

        foreach (string dir in remove)
        {
            if (directories.Contains(dir))
            {
                directories.Remove(dir);
            }
        }

        // if (directories.Count != 1)
        // {
        //     // error
        // }

        return directories.First();
    }
    // 5
    // get file extensions
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

        return fileExtensions;
    }

    // 6
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

    // get item(subject eg Banana) from file
    private HashSet<ItemModel> GetItemsClassification(HashSet<string> directories)
    {
        HashSet<ItemModel> items = new HashSet<ItemModel>();
        if (directories.Count != 0)
        {
            foreach (string dir in directories)
            {
                ItemModel item = new ItemModel
                {
                    Name = dir
                };
                items.Add(item);
            }
        }
        return items;
    }

    // get item
    private ItemModel GetItem(FileModel file)
    {
        string dir = GetDirectory(file);

        ItemModel savedItem = SaveItem(new ItemModel
        {
            Name = dir
        });

        return savedItem;
    }

    // save item to db
    private ItemModel SaveItem(ItemModel item)
    {
        if (_itemRepository.ExistsByName(item.Name))
        {
            return _itemRepository.GetItemByName(item.Name);
        }

        return _itemRepository.SaveItem(item);
    }

    // link files and items

    private HashSet<FileModel> GetAllFiles()
    {
        return _filesRepository.GetFiles();
    }

    public async Task<GetItemsResponseDTO> GetItems()
    {
        List<ItemDTO> itemDTOs = GetAllItems()
            .Select(ItemDTOConverter.ConvertToDTO)
            .ToList();

        return new GetItemsResponseDTO
        {
            Items = itemDTOs
        };
    }
    private HashSet<ItemModel> GetAllItems()
    {
        return _itemRepository.GetItems();
    }
}