public interface IFilesRepository
{
    // FileModel SaveFile(FileModel file);
    // HashSet<FileModel> GetFiles();
    // bool ExistsById(int id);
    // FileModel GetFileById(int id);

    Task<ICollection<FileModel>> GetFiles();
    FileModel GetFile(int id);
    // Task<bool> ExistsById(int id);
    bool ExistsById(int id);
    Task<bool> Save(FileModel file);
}