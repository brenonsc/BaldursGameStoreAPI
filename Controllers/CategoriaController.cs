using BaldursGame.Model;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

[Route("~/api/categorias")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly IValidator<Categoria> _categoriaValidator;
    
    public CategoriaController(ICategoriaService categoriaService, IValidator<Categoria> categoriaValidator)
    {
        _categoriaService = categoriaService;
        _categoriaValidator = categoriaValidator;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _categoriaService.GetAll());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var Categoria = await _categoriaService.GetById(id);
            
        if (Categoria == null)
            return NotFound();
        
        return Ok(Categoria);
    }
    
    [HttpGet("tipo/{tipo}")]
    public async Task<ActionResult> GetByTipo(string tipo)
    {
        return Ok(await _categoriaService.GetByTipo(tipo));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Categoria categoria)
    {
        var validationResult = await _categoriaValidator.ValidateAsync(categoria);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        await _categoriaService.Create(categoria);
        return CreatedAtAction(nameof(GetById), new {id = categoria.Id}, categoria);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Categoria categoria)
    {
        if (categoria.Id <= 0)
            return BadRequest("Id da categoria inválido");
        
        var validationResult = await _categoriaValidator.ValidateAsync(categoria);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var categoriaUpdate = await _categoriaService.Update(categoria);
        
        if (categoriaUpdate == null)
            return NotFound("Produto não encontrado");
        
        return Ok(categoriaUpdate);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var categoria = await _categoriaService.GetById(id);
        
        if (categoria == null)
            return NotFound("Produto não encontrado");
        
        await _categoriaService.Delete(categoria);
        return NoContent();
    }
}