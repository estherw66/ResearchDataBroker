using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ResearchDataBroker.Models;

public class FileModel
{
    [Key]
    [Required]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [Column("filename")]
    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public string Filename { get; set; }

    [Required]
    [Column("link")]
    [StringLength(maximumLength: 150, MinimumLength = 2)]
    public string Link { get; set; }

    [Required]
    [Column("directory_label")]
    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public string DirectoryLabel { get; set; }

    [Column("parent_id")]
    public int? ParentId { get; set; }

    public ICollection<ItemModel> Items { get; set; } = new List<ItemModel>();
}