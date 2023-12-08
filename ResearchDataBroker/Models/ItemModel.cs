using System.ComponentModel.DataAnnotations;

public class ItemModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 45, MinimumLength = 2)]
    public string Name { get; set; }
    public List<FileModel> Files { get; } = new();
}