using BaldursGame.Model;

namespace BaldursGame.Security;

public interface IAuthService
{
    Task<UserLogin?> Autenticar(UserLogin userLogin);
}