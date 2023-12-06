public class FileModel
{
    public int Id { get; set; }
    public string Filename { get; set; }
    public int Link { get; set; }
    public int? ParentId { get; set; }
    public List<ItemModel> Items { get; set; } = new();
}