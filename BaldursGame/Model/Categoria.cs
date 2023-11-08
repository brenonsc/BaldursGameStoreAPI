using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaldursGame.Filters;

namespace BaldursGame.Model;

public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [SwaggerIgnore]
    public long Id { get; set; }
    
    /// <summary>
    /// Tipo da categoria
    /// </summary>
    /// <example>Categoria</example>
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Tipo { get; set; } = string.Empty;
    
    [SwaggerIgnore]
    [InverseProperty("Categoria")]
    public virtual ICollection<Produto>? Produto { get; set; } = new List<Produto>();
}