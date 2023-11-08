using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaldursGame.Filters;
using BaldursGame.Util;
using Newtonsoft.Json;

namespace BaldursGame.Model;

public class Produto
{
    //Primary Key
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [SwaggerIgnore]
    public long Id { get; set; }
    
    /// <summary>
    /// Título do jogo
    /// </summary>
    /// <example>Título do jogo</example>
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;
    
    /// <summary>
    /// Descrição do jogo
    /// </summary>
    /// <example>Descrição do jogo</example>
    [Column(TypeName = "varchar")]
    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Console do jogo
    /// </summary>
    /// <example>PlayStation 5</example>
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Console { get; set; } = string.Empty;
    
    /// <summary>
    /// Data de lançamento do jogo
    /// </summary>
    /// <example>2000-01-01</example>
    [Column(TypeName = "date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime DataLancamento { get; set; }
    
    /// <summary>
    /// Preço do jogo
    /// </summary>
    /// <example>299.90</example>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
    
    /// <summary>
    /// Link para imagem do jogo
    /// </summary>
    /// <example></example>
    [Column(TypeName = "varchar")]
    [StringLength(200)]
    public string Imagem { get; set; } = string.Empty;
    
    [SwaggerIgnore]
    //Foreign Key
    public virtual Categoria? Categoria { get; set; }
}