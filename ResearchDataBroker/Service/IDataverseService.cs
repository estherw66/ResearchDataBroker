public interface IDataverseService
{
    Task<DataverseLatestVersionModel> GetLatestVersion(string url);
    Task<string> GetItemFromXml(string url);
}