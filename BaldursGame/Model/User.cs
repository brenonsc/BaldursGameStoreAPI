using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaldursGame.Filters;
using BaldursGame.Util;
using Newtonsoft.Json;

namespace BaldursGame.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [SwaggerIgnore]
    public long Id { get; set; }
    
    /// <summary>
    /// Nome do usuário
    /// </summary>
    /// <example>João Teste</example>
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Nome { get; set; } = string.Empty;    
    
    /// <summary>
    /// E-mail do usuário
    /// </summary>
    /// <example>example@email.com</example>
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Senha do usuário
    /// </summary>
    /// <example>Y0ur$tr0ngP@ssw0rd</example>
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Senha { get; set; } = string.Empty;
    
    /// <summary>
    /// Link para foto do usuário
    /// </summary>
    /// <example></example>
    [Column(TypeName = "varchar")]
    [StringLength(5000)]
    public string? Foto { get; set; } = string.Empty;
    
    /// <summary>
    /// Data de nascimento do usuário
    /// </summary>
    /// <example>2000-01-01</example>
    [Column(TypeName = "date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime DataNascimento { get; set; }
}