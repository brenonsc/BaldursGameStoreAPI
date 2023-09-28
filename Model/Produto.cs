using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaldursGame.Model;

public class Produto
{
    //Primary Key
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar")]
    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Console { get; set; } = string.Empty;
    
    [Column(TypeName = "date")]
    public DateOnly DataLancamento { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(200)]
    public string Imagem { get; set; } = string.Empty;
    
    //Foreign Key
    public virtual Categoria? Categoria { get; set; }
}