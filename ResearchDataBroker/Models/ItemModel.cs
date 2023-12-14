using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchDataBroker.Models;

public class ItemModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [StringLength(maximumLength: 45, MinimumLength = 2)]
    public string Name { get; set; }

    public ICollection<FileModel> Files { get; set; } = new List<FileModel>();
}