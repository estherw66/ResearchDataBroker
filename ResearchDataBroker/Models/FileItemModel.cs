public class FileItemModel
{
    public int Id { get; set; }
    public virtual FileModel File { get; set; }
    public virtual ItemModel Item { get; set; }
}