using System.ComponentModel.DataAnnotations;

public class FileModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public string Filename { get; set; }

    [Required]
    [StringLength(maximumLength: 150, MinimumLength = 2)]
    public string Link { get; set; }

    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public string DirectoryLabel { get; set; }

    public int? ParentId { get; set; }
    public List<ItemModel>? Items { get; } = new();
}