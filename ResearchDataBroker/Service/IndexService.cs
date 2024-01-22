using System.Collections;
using Microsoft.EntityFrameworkCore;
using ResearchDataBroker.Models;

namespace ResearchDataBroker.Service;

public class IndexService : IIndexService
{
    private readonly IDataverseService _dataverseService;
    private readonly IFilesRepository _filesRepository;
    private readonly IItemRepository _itemRepository;
    

    public IndexService(IDataverseService dataverseService, IItemRepository itemRepository, IFilesRepository filesRepository)
    {
        _dataverseService = dataverseService;
        _itemRepository = itemRepository;
        _filesRepository = filesRepository;
    }

    public async Task<IndexDatasetResponseDTO> IndexDataset(GetDatasetLatestVersionRequestDTO latestVersionRequest)
    {
        // 1: get dataset from dataverse
        DataverseLatestVersionModel latestVersion =
            await _dataverseService.GetLatestVersion(latestVersionRequest.DatasetUrl);
        
        // 2: map dataset to file models
        ICollection<FileModel> files = GetFileModels(latestVersion);
        
        // 3: get items from files (only classification for now)
        DatasetType type = CheckDatasetType(files);
        if (type != DatasetType.Classification)
        {
            return null;
        }

        // 4: save file and item
        ICollection<ItemModel> items = new HashSet<ItemModel>();
        foreach (FileModel file in files)
        {
            ItemModel item = GetItem(file);

            if (!item.Files.Any(f => f.Id == file.Id))
            {
                item.Files.Add(file);
            }

            if (!file.Items.Any(i => i.Name == item.Name))
            {
                file.Items.Add(item);
            }
            
            await SaveItem(item);
            await SaveFile(file);
            items.Add(item);
        }

        ICollection<ItemDTO> itemDTOs = items.Select(i => ItemDTOConverter.ConvertToDTO(i)).ToHashSet();
        
        IndexDatasetResponseDTO response = new IndexDatasetResponseDTO
        {
            ItemDTOs = itemDTOs
        };
        return response;
    }

    public async Task<GetFilesResponseDTO> GetFiles()
    {
        ICollection<FileDTO> fileDTOs = (await GetAllFiles())
            .Select(f => FileDTOConverter.ConvertToDTO(f))
            .ToHashSet();
        
        return new GetFilesResponseDTO
        {
            Files = fileDTOs
        };
    }

    public async Task<GetItemsResponseDTO> GetItems()
    {
        ICollection<ItemDTO> itemDTOs = (await GetAllItems())
            .Select(i => ItemDTOConverter.ConvertToDTO(i))
            .ToHashSet();

        return new GetItemsResponseDTO
        {
            Items = itemDTOs
        };
    }
    
    // index dataset
    // 2: map dataset to file models
    private ICollection<FileModel> GetFileModels(DataverseLatestVersionModel latestVersion)
    {
        ICollection<FileModel> fileModels = new List<FileModel>();
        foreach (DataverseFileModel file in latestVersion.Files)
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
    
    // 3: get items
    private ItemModel GetItem(FileModel file)
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
    
    // save file
    private async Task<bool> SaveFile(FileModel file)
    {
        return await _filesRepository.Save(file);
    }
    
    // save item
    private async Task<bool> SaveItem(ItemModel item)
    {
        return await _itemRepository.Save(item);
    }
    
    private DatasetType CheckDatasetType(ICollection<FileModel> files)
    {
        ICollection<string> directories = new List<string>();
        foreach (FileModel file in files)
        {
            string dir = GetDirectory(file);
            directories.Add(dir);
        }
        ICollection<string> fileExtensions = GetFileExtensions(files);

        if (directories.Count != 0)
        {
            return DatasetType.Classification;
        }

        // add other image file types
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

    // remove method
    private string GetDirectory(FileModel file)
    {
        ICollection<string> directories = new HashSet<string>();
        
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

    private ICollection<string> GetFileExtensions(ICollection<FileModel> files)
    {
        ICollection<string> fileExtensions = new HashSet<string>();

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
    
    // get files
    private async Task<ICollection<FileModel>> GetAllFiles()
    {
        return await _filesRepository.GetFiles();
    }
    
    // get items 
    private async Task<ICollection<ItemModel>> GetAllItems()
    {
        return await _itemRepository.GetItems();
    }
}