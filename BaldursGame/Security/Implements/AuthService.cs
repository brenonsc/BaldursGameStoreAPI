using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BaldursGame.Model;
using BaldursGame.Service;
using Microsoft.IdentityModel.Tokens;

namespace BaldursGame.Security.Implements;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<UserLogin?> Autenticar(UserLogin userLogin)
    {
        string fotoDefault = "https://i.pinimg.com/originals/57/00/c0/5700c04197ee9a4372a35ef16eb78f4e.png";
        
        if(userLogin is null || string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Senha))
            return null;
        
        var buscaUsuario = await _userService.GetByEmail(userLogin.Email);
        
        if(buscaUsuario is null)
            return null;
        
        if(!BCrypt.Net.BCrypt.Verify(userLogin.Senha, buscaUsuario.Senha))
            return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(Settings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userLogin.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        userLogin.Id = buscaUsuario.Id;
        userLogin.Nome = buscaUsuario.Nome;
        userLogin.Foto = buscaUsuario.Foto is null ? fotoDefault : buscaUsuario.Foto;
        userLogin.Token = "Bearer " + tokenHandler.WriteToken(token).ToString();
        userLogin.Senha = "";
        
        return userLogin;
    }
}