using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [StringLength(maximumLength: 45, MinimumLength = 2)]
    public string Name { get; set; }
    
    public List<FileModel> Files { get; } = new();
}