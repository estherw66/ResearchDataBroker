public interface IIndexService
{
    Task<List<ItemDTO>> IndexDataset(GetDatasetRequestDTO request);
    Task<GetFilesResponseDTO> GetFiles();
    Task<GetItemsResponseDTO> GetItems();
}