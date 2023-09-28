using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaldursGame.Model;

public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Tipo { get; set; } = string.Empty;
    
    [InverseProperty("Categoria")]
    public virtual ICollection<Produto>? Produto { get; set; } = new List<Produto>();
}