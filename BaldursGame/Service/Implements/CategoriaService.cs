using BaldursGame.Data;
using BaldursGame.Model;
using Microsoft.EntityFrameworkCore;

namespace BaldursGame.Service.Implements;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;
    
    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Categoria>> GetAll()
    {
        return await _context.Categorias
            .Include(c => c.Produto)
            .ToListAsync();
    }

    public async Task<Categoria?> GetById(long id)
    {
        try
        {
            var categoria = await _context.Categorias
                .Include(c => c.Produto)
                .FirstAsync(i => i.Id == id);
            return categoria;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Categoria>> GetByTipo(string tipo)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Produto)
            .Where(c => c.Tipo.Contains(tipo)).ToListAsync();
        return categoria;
    }

    public async Task<Categoria?> Create(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
        
        return categoria;
    }

    public async Task<Categoria?> Update(Categoria categoria)
    {
        var categoriaUpdate = await _context.Categorias.FindAsync(categoria.Id);

        if (categoriaUpdate == null)
            return null;
        
        _context.Entry(categoriaUpdate).State = EntityState.Detached;
        _context.Entry(categoria).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return categoria;
    }

    public async Task Delete(Categoria categoria)
    {
        _context.Remove(categoria);
        
        await _context.SaveChangesAsync();
    }
}