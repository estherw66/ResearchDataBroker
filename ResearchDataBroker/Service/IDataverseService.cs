public interface IDataverseService
{
    Task<DataverseLatestVersionModel> GetLatestVersion(string url);
}