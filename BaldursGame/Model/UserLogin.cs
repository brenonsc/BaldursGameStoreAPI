using BaldursGame.Filters;

namespace BaldursGame.Model;

public class UserLogin
{
    [SwaggerIgnore]
    public long Id { get; set; }
    
    [SwaggerIgnore]
    public string Nome { get; set; } = string.Empty;    

    /// <summary>
    /// E-mail do usuário
    /// </summary>
    /// <example>example@email.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    /// <example>Y0ur$tr0ngP@ssw0rd</example>
    public string Senha { get; set; } = string.Empty;

    [SwaggerIgnore]
    public string Foto { get; set; } = string.Empty;
    
    [SwaggerIgnore]
    public DateTime DataNascimento { get; set; }
    
    [SwaggerIgnore]
    public string Token { get; set; } = string.Empty;
}