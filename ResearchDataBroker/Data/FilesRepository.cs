public class FilesRepository : IFilesRepository
{
    public static HashSet<FileModel> Files { get; } = new();
    public static int FileId { get; private set; } = 1;

    public FileModel SaveFile(FileModel file)
    {
        if (ExistsById(file.Id))
        {
            return GetFileById(file.Id);
        }

        Files.Add(file);
        return file;
    }

    public HashSet<FileModel> GetFiles()
    {
        return Files;
    }

    public bool ExistsById(int id)
    {
        return Files.Any(file => file.Id == id);
    }

    public FileModel GetFileById(int id)
    {
        if (ExistsById(id))
        {
            FileModel file = Files.First(file => file.Id == id);
            return file;
        }

        return null;        
    }
}