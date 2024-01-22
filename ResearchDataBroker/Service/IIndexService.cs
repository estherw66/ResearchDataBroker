using ResearchDataBroker.DTO;

public interface IIndexService
{
    Task<IndexDatasetResponseDTO> IndexDataset(GetDatasetLatestVersionRequestDTO latestVersionRequest);
    Task<GetFilesResponseDTO> GetFiles();
    Task<GetItemsResponseDTO> GetItems();
    Task<GetFilesResponseDTO> GetFilesByItem(string itemName);
    // ItemDTO GetItemByFilename(string filename);
}