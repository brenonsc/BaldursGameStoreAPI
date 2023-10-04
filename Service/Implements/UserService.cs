using BaldursGame.Data;
using BaldursGame.Model;
using Microsoft.EntityFrameworkCore;

namespace BaldursGame.Service.Implements;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task<User?> GetById(long id)
    {
        try
        {
            var usuario = await _context.Users
                .FirstAsync(i => i.Id == id);
            
            usuario.Senha = "";
            
            return usuario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> GetByEmail(string email)
    {
        try
        {
            // SELECT * FROM tb_usuarios WHERE Email = email
            var buscaUsuario = await _context.Users
                .Where(u => u.Email == email).FirstOrDefaultAsync();

            return buscaUsuario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> Create(User usuario)
    {
        var buscaUsuario = await GetByEmail(usuario.Email);
        
        if(buscaUsuario is not null)
            return null;
        
        if(string.IsNullOrEmpty(usuario.Foto))
            usuario.Foto = "https://i.pinimg.com/originals/57/00/c0/5700c04197ee9a4372a35ef16eb78f4e.png";
        
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);
        
        await _context.Users.AddAsync(usuario);
        await _context.SaveChangesAsync();
        
        return usuario;
    }

    public async Task<User?> Update(User usuario)
    {
        var userUpdate = await _context.Users.FindAsync(usuario.Id);

        if (userUpdate == null)
            return null;
        
        if(string.IsNullOrEmpty(usuario.Foto))
            usuario.Foto = "https://i.pinimg.com/originals/57/00/c0/5700c04197ee9a4372a35ef16eb78f4e.png";
        
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);
        
        _context.Entry(userUpdate).State = EntityState.Detached;
        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return usuario;
    }
}