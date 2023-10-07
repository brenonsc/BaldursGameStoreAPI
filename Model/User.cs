using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaldursGame.Util;
using Newtonsoft.Json;

namespace BaldursGame.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Nome { get; set; } = string.Empty;    
    
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Senha { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar")]
    [StringLength(5000)]
    public string? Foto { get; set; } = string.Empty;
    
    [Column(TypeName = "date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime DataNascimento { get; set; }
}