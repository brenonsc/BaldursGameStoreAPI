using BaldursGame.Model;

namespace BaldursGame.Service;

public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> GetAll();
    
    Task<Categoria?> GetById(long id);
    
    Task<IEnumerable<Categoria>> GetByTipo(string descricao);
    
    Task<Categoria?> Create(Categoria categoria);
    
    Task<Categoria?> Update(Categoria categoria);
    
    Task Delete(Categoria categoria);
}