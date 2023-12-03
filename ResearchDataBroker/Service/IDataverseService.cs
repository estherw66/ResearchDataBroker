public interface IDataverseService
{
    Task<DataverseLatestVersionModel> GetLatestVersion(GetDatasetRequestDTO request);
}