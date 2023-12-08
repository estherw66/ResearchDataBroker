public interface IIndexService
{
    Task<IndexDatasetResponseDTO> IndexDataset(GetDatasetRequestDTO request);
    Task<GetFilesResponseDTO> GetFiles();
    Task<GetItemsResponseDTO> GetItems();
}