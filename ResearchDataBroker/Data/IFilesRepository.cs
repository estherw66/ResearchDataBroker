public interface IFilesRepository
{
    // FileModel SaveFile(FileModel file);
    // HashSet<FileModel> GetFiles();
    // bool ExistsById(int id);
    // FileModel GetFileById(int id);

    Task<ICollection<FileModel>> GetFiles();
    FileModel GetFile(int id);

    FileModel GetFileByName(string name);
    // Task<bool> ExistsById(int id);
    bool ExistsById(int id);
    // bool ExistsByName(string name);
    Task<bool> Save(FileModel file);
    Task<ICollection<FileModel>> GetFilesByItem(string itemName);
}