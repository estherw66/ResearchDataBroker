public class ItemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<FileModel> Files { get; set; } = new();
}