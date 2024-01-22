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

    public FileModel GetFileByName(string name)
    {
        if (!ExistsByName(name))
        {
            return null;
        }

        return _context.Files.FirstOrDefault(f => f.Filename == name);
    }

    public bool ExistsById(int id)
    {
        return _context.Files.Any(f => f.Id == id);
    }

    public bool ExistsByName(string name)
    {
        return _context.Files.Any(f => f.Filename == name);
    }

    public async Task<bool> Save(FileModel file)
    {
        if (ExistsById(file.Id))
        {
            // todo update file
            // todo fix tracking ? 
            
            // var existingFile = GetFile(file.Id);
            // existingFile.ParentId = file.ParentId;
            //
            // var updated = await _context.SaveChangesAsync();
            // return updated > 0;
            return true;
        }
        _context.Files.Add(file);
        var saved =  await _context.SaveChangesAsync();
        return saved > 0;
    }

    public async Task<ICollection<FileModel>> GetFilesByItem(string itemName)
    {
        return await _context.Files
            .Include(f => f.Items)
            .Where(f => f.Items.Any(i => i.Name == itemName))
            .ToListAsync();
    }
}