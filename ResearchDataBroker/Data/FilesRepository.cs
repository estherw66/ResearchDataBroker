using Microsoft.EntityFrameworkCore;

public class FilesRepository : IFilesRepository
{
    private readonly AppDBContext _context;

    public FilesRepository(AppDBContext context)
    {
        _context = context;
    }

    // public static HashSet<FileModel> Files { get; } = new();
    // public static int FileId { get; private set; } = 1;
    //
    // public FileModel SaveFile(FileModel file)
    // {
    //     if (ExistsById(file.Id))
    //     {
    //         return GetFileById(file.Id);
    //     }
    //
    //     Files.Add(file);
    //     return file;
    // }
    //
    // public HashSet<FileModel> GetFiles()
    // {
    //     return Files;
    // }
    //
    // public bool ExistsById(int id)
    // {
    //     return Files.Any(file => file.Id == id);
    // }
    //
    // public FileModel GetFileById(int id)
    // {
    //     if (ExistsById(id))
    //     {
    //         FileModel file = Files.First(file => file.Id == id);
    //         return file;
    //     }
    //
    //     return null;        
    // }
    public async Task<ICollection<FileModel>> GetFiles()
    {
        return await _context.Files.ToListAsync();
    }

    public FileModel GetFile(int id)
    {
        if (ExistsById(id))
        {
            return _context.Files.First(f => f.Id == id);
        }

        return null;
    }

    public bool ExistsById(int id)
    {
        return _context.Files.Any(f => f.Id == id);
    }

    public async Task<bool> Save(FileModel file)
    {
        if (ExistsById(file.Id))
        {
            return true;
        }
        _context.Files.Add(file);
        var saved =  await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
}