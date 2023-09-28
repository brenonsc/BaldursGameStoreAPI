using BaldursGame.Model;

namespace BaldursGame.Service;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> GetAll();
    
    Task<Produto?> GetById(long id);
    
    Task<IEnumerable<Produto>> GetByTitulo(string titulo);
    
    Task<IEnumerable<Produto>> GetByTitleOrConsole(string texto);
    
    Task<IEnumerable<Produto>> GetByPriceRange(decimal min, decimal max);
    
    Task<Produto?> Create(Produto produto);
    
    Task<Produto?> Update(Produto produto);
    
    Task Delete(Produto produto);
}