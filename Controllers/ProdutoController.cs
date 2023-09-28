using BaldursGame.Model;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

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
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _produtoService.GetAll());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var produto = await _produtoService.GetById(id);
            
        if (produto == null)
            return NotFound();
        
        return Ok(produto);
    }
    
    [HttpGet("titulo/{titulo}")]
    public async Task<ActionResult> GetByTitulo(string titulo)
    {
        return Ok(await _produtoService.GetByTitulo(titulo));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Produto produto)
    {
        var validationResult = await _produtoValidator.ValidateAsync(produto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var resposta = await _produtoService.Create(produto);
        
        if (resposta == null)
            return BadRequest("Categoria não encontrada");
        
        return CreatedAtAction(nameof(GetById), new {id = produto.Id}, produto);
    }

    [HttpPut ("{id}")]
    public async Task<ActionResult> Update([FromBody] Produto produto)
    {
        if (produto.Id <= 0)
            return BadRequest("Id do produto inválido");
        
        var validationResult = await _produtoValidator.ValidateAsync(produto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var produtoUpdate = await _produtoService.Update(produto);
        
        if (produtoUpdate == null)
            return NotFound("Produto e/ou categoria não encontrado(s)");
        
        return Ok(produtoUpdate);
    }
    
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