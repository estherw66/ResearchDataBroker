public class ItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> FileIds { get; } = new();
}