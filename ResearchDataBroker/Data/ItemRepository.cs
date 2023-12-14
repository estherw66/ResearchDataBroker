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

    // public static HashSet<ItemModel> Items { get; } = new();
    // public static int ItemId { get; set; } = 1;
    //
    // public ItemModel SaveItem(ItemModel item)
    // {
    //     if (ExistsByName(item.Name))
    //     {
    //         return GetItemByName(item.Name);
    //     }
    //
    //     item.Id = ItemId;
    //     ItemId++;
    //
    //     Items.Add(item);
    //     return item;
    // }
    //
    // public HashSet<ItemModel> GetItems()
    // {
    //     return Items;
    // }
    //
    // public bool ExistsByName(string name)
    // {
    //     return Items.Any(item => item.Name == name);
    // }
    //
    // public ItemModel GetItemByName(string name)
    // {
    //     ItemModel item = Items.First(item => item.Name == name);
    //     if (item != null)
    //     {
    //         return item;
    //     }
    //
    //     return null;
    // }
    public async Task<ICollection<ItemModel>> GetItems()
    {
        return await _context.Items.ToListAsync();
    }

    public ItemModel GetItemByName(string name)
    {
        if (ExistsByName(name))
        {
            return _context.Items.First(i => i.Name == name);
        }

        return null;
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
        return saved > 0 ? true : false;
    }
}