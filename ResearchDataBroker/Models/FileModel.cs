public class FileModel
{
    public int Id { get; set; }
    public string Filename { get; set; }
    public string Link { get; set; }
    public string DirectoryLabel { get; set; }
    public int? ParentId { get; set; }
    public List<ItemModel> Items { get; set; } = new();
}