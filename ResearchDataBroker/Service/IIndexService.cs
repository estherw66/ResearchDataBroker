public interface IIndexService
{
    Task<List<ItemModel>> IndexDataset(GetDatasetRequestDTO request);
    Task<List<FileModel>> GetFileModels(GetDatasetRequestDTO request);
}