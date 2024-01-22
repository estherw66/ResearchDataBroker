using ResearchDataBroker.Models;

public interface IItemRepository
{
    Task<ICollection<ItemModel>> GetItems();
    ItemModel? GetItemByName(string name);
    ItemModel GetItemFromFilename(string filename);
    bool ExistsByName(string name);
    Task<bool> Save(ItemModel? item);
    ItemModel GetItemByFile(string filename);
}