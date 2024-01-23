using ResearchDataBroker.Models;

namespace ResearchDataBroker.Service;

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

    public async Task<IndexDatasetResponseDTO> IndexDataset(GetDatasetLatestVersionRequestDTO latestVersionRequest)
    {
        // 1. get file models
        ICollection<FileModel> files = await GetFileModels(latestVersionRequest.DatasetUrl);
        
        // 2. check dataset type
        DatasetType type = CheckDatasetType(files);
        
        // 3. save file and item
        ICollection<ItemModel> items = new HashSet<ItemModel>();
        foreach (var file in files)
        {
            if (type == DatasetType.Classification)
            {
                ItemModel item = GetItemClassification(file);
                items = await SaveFileItems(item, file, items);
            } else if (type == DatasetType.ObjectDetection)
            {
                string ext = GetFileExtension(file);

                if (ext == ".xml")
                {
                    ItemModel item = await GetItemObjectDetection(file);
                    items = await SaveFileItems(item, file, items);
                }
                
            } else if (type == DatasetType.Tabular)
            {
                throw new NotImplementedException();
            } else if (type == DatasetType.Other)
            {
                throw new NotImplementedException();
            }
        }

        foreach (var file in files)
        {
            if (type == DatasetType.ObjectDetection)
            {
                string ext = GetFileExtension(file);

                if (ext == ".jpg")
                {
                    string filename = file.Filename.Replace(".jpg", ".xml");
                    ItemModel item = GetItemByFilename(filename);
                    items = await SaveFileItems(item, file, items);
                }

                if (ext == ".xml")
                {
                    await LinkFileToParent(file);
                }
            }
        }

        if (items.Count != 0)
        {
            ICollection<ItemDTO> itemDtos = items.Select(i => ItemDTOConverter.ConvertToDTO(i))
                .ToHashSet();

            IndexDatasetResponseDTO response = new IndexDatasetResponseDTO
            {
                ItemDTOs = itemDtos
            };

            return response;
        }

        return null;
    }

    public async Task<GetFilesResponseDTO> GetFiles()
    {
        ICollection<FileDTO> fileDtos = (await _filesRepository.GetFiles())
            .Select(f => FileDTOConverter.ConvertToDTO(f))
            .ToHashSet();

        return new GetFilesResponseDTO
        {
            Files = fileDtos
        };
    }

    public async Task<GetItemsResponseDTO> GetItems()
    {
        ICollection<ItemDTO> itemDtos = (await _itemRepository.GetItems())
            .Select(i => ItemDTOConverter.ConvertToDTO(i))
            .ToHashSet();

        return new GetItemsResponseDTO
        {
            Items = itemDtos
        };
    }

    public async Task<GetFilesResponseDTO> GetFilesByItem(string itemName)
    {
        ICollection<FileDTO> fileDtos = (await _filesRepository.GetFilesByItem(itemName))
            .Select(f => FileDTOConverter.ConvertToDTO(f))
            .ToHashSet();

        return new GetFilesResponseDTO
        {
            Files = fileDtos
        };
    }

    // public ItemDTO GetItemByFilename(string filename)
    // {
    //     var item = _itemRepository.GetItemFromFilename(filename);
    //
    //     return ItemDTOConverter.ConvertToDTO(item);
    // }

    // 1. get file models
    private async Task<ICollection<FileModel>> GetFileModels(string url)
    {
        DataverseLatestVersionModel latestVersionModel = await _dataverseService.GetLatestVersion(url);
        
        ICollection<FileModel> fileModels = new List<FileModel>();
        foreach (DataverseFileModel file in latestVersionModel.Files)
        {
            if (file.DirectoryLabel.Contains("data", StringComparison.OrdinalIgnoreCase))
            {
                FileModel fileModel = new FileModel
                {
                    Id = file.DataFile.Id,
                    Filename = file.DataFile.Filename,
                    Link = $"{ServerConfig.ServerUrl}/api/access/datafile/{file.DataFile.Id}",
                    DirectoryLabel = file.DirectoryLabel.ToLower()
                };
                
                fileModels.Add(fileModel);
            }
        }

        return fileModels;
    }
    
    // 2. check dataset type
    private DatasetType CheckDatasetType(ICollection<FileModel> files)
    {
        ICollection<string> directories = new HashSet<string>();
        ICollection<string> fileExtensions = new HashSet<string>();

        foreach (FileModel file in files)
        {
            string dir = GetDirectory(file);
            if (dir != null)
            {
                directories.Add(dir);
            }

            string fileExtension = GetFileExtension(file);
            fileExtensions.Add(fileExtension);
        }

        if (directories.Count != 0)
        {
            return DatasetType.Classification;
        } else if (fileExtensions.Contains(".xml") && fileExtensions.Contains(".jpg"))
        {
            // TODO add more file extensions for images
            return DatasetType.ObjectDetection;
        } else if (fileExtensions.Contains(".csv"))
        {
            return DatasetType.Tabular;
        }
        else
        {
            return DatasetType.Other;
        }
    }
    
    // 3. save file and item
    
    // get items
    private ItemModel GetItemClassification(FileModel file)
    {
        string dir = GetDirectory(file);
        var item = _itemRepository.GetItemByName(dir);

        if (item == null)
        {
            return new ItemModel
            {
                Name = dir
            };
        }

        return item;
    }

    private async Task<ItemModel> GetItemObjectDetection(FileModel file)
    {
        string name = await _dataverseService.GetItemFromXml(file.Link);
        var item = _itemRepository.GetItemByName(name);

        if (item == null)
        {
            return new ItemModel
            {
                Name = name
            };
        }

        return item;
    }

    private ItemModel GetItemTabular(FileModel file)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> SaveItem(ItemModel item)
    {
        return await _itemRepository.Save(item);
    }

    private async Task<bool> SaveFile(FileModel file)
    {
        return await _filesRepository.Save(file);
    }

    private async Task<ICollection<ItemModel>> SaveFileItems(ItemModel item, FileModel file, ICollection<ItemModel> items)
    {
        if (item != null)
        {
            if (!item.Files.Any(f => f.Id == file.Id))
            {
                item.Files.Add(file);
            }

            if (!file.Items.Any(i => i.Id == item.Id))
            {
                file.Items.Add(item);
            }

            await SaveItem(item);
            await SaveFile(file);
            items.Add(item);

            return items;
        }

        return items;
    }
    
    // link parent id
    private async Task LinkFileToParent(FileModel file)
    {
        string parentFilename = file.Filename.Replace(".xml", ".jpg");
        FileModel parent = _filesRepository.GetFileByName(parentFilename);
        file.ParentId = parent.Id;

        await SaveFile(file);
    }
    
    private string GetDirectory(FileModel file)
    {
        ICollection<string> directories = new HashSet<string>();

        string[] directoryParts = file.DirectoryLabel.Split('/');
        foreach (var part in directoryParts)
        {
            directories.Add(part.ToLower());
        }

        string[] remove = new string[4] { "data", "train", "test", "eval" };

        foreach (var dir in remove)
        {
            if (directories.Contains(dir))
            {
                directories.Remove(dir);
            }
        }

        if (directories.Count != 1)
        {
            // TODO check FDD for directory rules
            // TODO throw error
            return null;
        }

        return directories.First();
    }

    private string GetFileExtension(FileModel file)
    {
        if (file.DirectoryLabel.Contains("data", StringComparison.OrdinalIgnoreCase))
        {
            FileInfo fi = new FileInfo(file.Filename);
            string ext = fi.Extension;
            return ext;
        }

        return null;
    }

    private ItemModel GetItemByFilename(string filename)
    {
        ItemModel item = _itemRepository.GetItemFromFilename(filename);

        if (item != null)
        {
            return item;
        }

        return null;
    }
}