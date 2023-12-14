using ResearchDataBroker.Models;

public interface IItemRepository
{
    Task<ICollection<ItemModel>> GetItems();
    ItemModel? GetItemByName(string name);
    bool ExistsByName(string name);
    Task<bool> Save(ItemModel? item);
}