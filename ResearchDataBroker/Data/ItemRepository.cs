using Microsoft.EntityFrameworkCore;
using ResearchDataBroker.Models;

namespace ResearchDataBroker.Data;

public class ItemRepository : IItemRepository
{
    private readonly AppDBContext _context;

    public ItemRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ItemModel>> GetItems()
    {
        return await _context.Items.Include(i => i.Files).ToListAsync();
        // return await _context.Items.ToListAsync();
    }

    public ItemModel GetItemByName(string name)
    {
        if (ExistsByName(name))
        {
            return _context.Items.First(i => i.Name == name);
        }

        return null;
    }

    public ItemModel GetItemFromFilename(string filename)
    {
        filename.Replace(".jpg", ".xml");
        ItemModel item = _context.Items.FirstOrDefault(i => i.Files.Any(f => f.Filename == filename));
        return item;
    }

    public bool ExistsByName(string name)
    {
        return _context.Items.Any(i => i.Name == name);
    }

    public async Task<bool> Save(ItemModel item)
    {
        if (ExistsByName(item.Name))
        {
            return true;
        }
        _context.Items.Add(item);
        var saved =  await _context.SaveChangesAsync();
        return saved > 0;
    }

    public ItemModel GetItemByFile(string filename)
    {
        return _context.Items
            .Include(i => i.Files)
            .FirstOrDefault(i => i.Files.Any(f => f.Filename == filename));
    }
}