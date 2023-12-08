using System.Collections.Generic;
using System.Linq;

public class ItemRepository : IItemRepository
{
    public static HashSet<ItemModel> Items { get; } = new();
    public static int ItemId { get; set; } = 1;
    
    public ItemModel SaveItem(ItemModel item)
    {
        if (ExistsByName(item.Name))
        {
            return GetItemByName(item.Name);
        }

        item.Id = ItemId;
        ItemId++;

        Items.Add(item);
        return item;
    }

    public HashSet<ItemModel> GetItems()
    {
        return Items;
    }

    public bool ExistsByName(string name)
    {
        return Items.Any(item => item.Name == name);
    }

    public ItemModel GetItemByName(string name)
    {
        ItemModel item = Items.First(item => item.Name == name);
        if (item != null)
        {
            return item;
        }

        return null;
    }
}