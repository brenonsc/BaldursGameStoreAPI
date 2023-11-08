using BaldursGame.Model;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

[Authorize]
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
    
    /// <summary>
    /// Retorna todas as categorias cadastradas no sistema
    /// </summary>
    /// <returns>Retorna todas as categorias cadastradas no sistema</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _categoriaService.GetAll());
    }
    
    /// <summary>
    /// Retorna a categoria com o ID informado
    /// </summary>
    /// <returns>Retorna atributos da categoria</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var Categoria = await _categoriaService.GetById(id);
            
        if (Categoria == null)
            return NotFound();
        
        return Ok(Categoria);
    }
    
    /// <summary>
    /// Retorna as categorias que contém o tipo informado
    /// </summary>
    /// <returns>Retorna atributos da categoria informada</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpGet("tipo/{tipo}")]
    public async Task<ActionResult> GetByTipo(string tipo)
    {
        return Ok(await _categoriaService.GetByTipo(tipo));
    }

    /// <summary>
    /// Cadastra uma nova categoria
    /// </summary>
    /// <returns>Retorna atributos da categoria informada</returns>
    /// <response code="201">Localidade criada com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Categoria categoria)
    {
        var validationResult = await _categoriaValidator.ValidateAsync(categoria);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        await _categoriaService.Create(categoria);
        return CreatedAtAction(nameof(GetById), new {id = categoria.Id}, categoria);
    }

    /// <summary>
    /// Atualiza uma categoria existente
    /// </summary>
    /// <returns>Retorna atributos da categoria informada</returns>
    /// <response code="200">Localidade atualizada com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Categoria categoria)
    {
        if (id <= 0)
            return BadRequest("ID da categoria inválido");

        if (categoria == null)
            return BadRequest("Dados de categoria inválidos");

        categoria.Id = id;

        // Verifique se a categoria com o ID especificado existe no banco de dados.
        var existingCategoria = await _categoriaService.GetById(id);

        if (existingCategoria == null)
            return NotFound("Categoria não encontrada");

        var validationResult = await _categoriaValidator.ValidateAsync(categoria);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Realize a atualização da categoria com os dados recebidos no corpo da requisição
        var categoriaUpdate = await _categoriaService.Update(categoria);

        if (categoriaUpdate == null)
            return NotFound("Não foi possível atualizar a categoria");

        return Ok(categoriaUpdate);
    }
    
    /// <summary>
    /// Deleta uma categoria existente pelo ID
    /// </summary>
    /// <returns>Retorna NoContent</returns>
    /// <response code="204">Localidade deletada com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
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