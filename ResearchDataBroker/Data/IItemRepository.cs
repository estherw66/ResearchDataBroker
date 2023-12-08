public interface IItemRepository
{
    ItemModel SaveItem(ItemModel item);
    HashSet<ItemModel> GetItems();
    bool ExistsByName(string name);
    ItemModel GetItemByName(string name);
}