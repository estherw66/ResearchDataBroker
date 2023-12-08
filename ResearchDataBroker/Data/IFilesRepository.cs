public interface IFilesRepository
{
    FileModel SaveFile(FileModel file);
    HashSet<FileModel> GetFiles();
    bool ExistsById(int id);
    FileModel GetFileById(int id);
}