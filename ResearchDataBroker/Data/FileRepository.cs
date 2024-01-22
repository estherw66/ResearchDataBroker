using Microsoft.EntityFrameworkCore;

namespace ResearchDataBroker.Data;

public class FileRepository : IFilesRepository
{
    private readonly AppDBContext _context;

    public FileRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<ICollection<FileModel>> GetFiles()
    {
        return await _context.Files.Include(f=>f.Items).ToListAsync();
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
        return saved > 0;
    }
}