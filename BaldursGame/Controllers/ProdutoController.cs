using BaldursGame.Model;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

[Authorize]
[Route("~/api/produtos")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IValidator<Produto> _produtoValidator;
    
    public ProdutoController(IProdutoService produtoService, IValidator<Produto> produtoValidator)
    {
        _produtoService = produtoService;
        _produtoValidator = produtoValidator;
    }
    
    /// <summary>
    /// Retorna todos os jogos cadastrados no sistema
    /// </summary>
    /// <returns>Todos os jogos cadastrados no sistema</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _produtoService.GetAll());
    }
    
    /// <summary>
    /// Retorna o jogo com o ID informado
    /// </summary>
    /// <returns>Atributos do jogo</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var produto = await _produtoService.GetById(id);
            
        if (produto == null)
            return NotFound();
        
        return Ok(produto);
    }
    
    /// <summary>
    /// Retorna os jogos que contém o título informado
    /// </summary>
    /// <returns>Atributos do jogo</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("titulo/{titulo}")]
    public async Task<ActionResult> GetByTitulo(string titulo)
    {
        return Ok(await _produtoService.GetByTitulo(titulo));
    }
    
    /// <summary>
    /// Retorna os jogos que estão no intervalo de preço informado
    /// </summary>
    /// <returns>Atributos dos jogos</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("preco/{min}/{max}")]
    public async Task<ActionResult> GetByPriceRange(decimal min, decimal max)
    {
        return Ok(await _produtoService.GetByPriceRange(min, max));
    }
    
    /// <summary>
    /// Retorna os jogos que contém o título ou console informado
    /// </summary>
    /// <returns>Atributos dos jogos</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("titulo/{titulo}/ouconsole/{console}")]
    public async Task<ActionResult> GetByTitleOrConsole(string titulo, string console)
    {
        return Ok(await _produtoService.GetByTitleOrConsole(titulo, console));
    }

    /// <summary>
    /// Cadastra um novo jogo
    /// </summary>
    /// <returns>Atributos do jogo cadastrado</returns>
    /// <response code="201">Jogo criado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpPost("{categoriaId}")]
    public async Task<ActionResult> Create(int categoriaId, [FromBody] Produto produto)
    {
        if (categoriaId <= 0)
            return BadRequest("categoriaId inválido");

        if (produto == null)
            return BadRequest("Dados do produto inválidos");

        // Inicialize a propriedade Categoria do objeto produto
        produto.Categoria = new Categoria { Id = categoriaId };

        var validationResult = await _produtoValidator.ValidateAsync(produto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Agora você pode continuar com a criação do produto associado à categoria.
        var resposta = await _produtoService.Create(produto);

        if (resposta == null)
            return BadRequest("Categoria não encontrada");

        // Retorne um status 201 Created com a localização do novo recurso.
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    /// <summary>
    /// Atualiza um jogo existente
    /// </summary>
    /// <returns>Atributos do jogo informado</returns>
    /// <response code="200">Jogo atualizado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpPut("{id}/categoria/{categoriaId}")]
    public async Task<ActionResult> Update(int id, int categoriaId, [FromBody] Produto produto)
    {
        if (id <= 0)
            return BadRequest("ID do produto inválido");

        if (produto == null)
            return BadRequest("Dados do produto inválidos");

        // Verifique se o produto com o ID especificado existe no banco de dados.
        var existingProduto = await _produtoService.GetById(id);

        if (existingProduto == null)
            return NotFound("Produto não encontrado");

        // Certifique-se de que o ID da categoria seja igual ao categoriaId especificado na URL.
        produto.Id = id;
        produto.Categoria = new Categoria { Id = categoriaId };

        var validationResult = await _produtoValidator.ValidateAsync(produto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Agora você pode continuar com a atualização do produto com a nova categoria associada.
        var produtoUpdate = await _produtoService.Update(produto);

        if (produtoUpdate == null)
            return NotFound("Não foi possível atualizar o produto");

        return Ok(produtoUpdate);
    }
    
    /// <summary>
    /// Deleta um jogo existente pelo ID
    /// </summary>
    /// <returns>NoContent</returns>
    /// <response code="204">Jogo deletado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var produto = await _produtoService.GetById(id);
        
        if (produto == null)
            return NotFound("Produto não encontrado");
        
        await _produtoService.Delete(produto);
        return NoContent();
    }
}