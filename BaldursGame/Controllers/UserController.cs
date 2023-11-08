using BaldursGame.Model;
using BaldursGame.Security;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

[Route("~/api/usuarios")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<User> _userValidator;
    private readonly IAuthService _authService;
    
    public UserController(IUserService userService, IValidator<User> userValidator, IAuthService authService)
    {
        _userService = userService;
        _userValidator = userValidator;
        _authService = authService;
    }
    
    /// <summary>
    /// Retorna todos os usuários cadastrados no sistema
    /// </summary>
    /// <returns>Todos os usuários cadastrados</returns>
    /// <response code="201">Usuário cadastrado com sucesso</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _userService.GetAll());
    }
    
    /// <summary>
    /// Buscar um usuário pelo ID
    /// </summary>
    /// <returns>Atributos do usuário</returns>
    /// <response code="201">Usuário cadastrado com sucesso</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var user = await _userService.GetById(id);
            
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

    /// <summary>
    /// Cria um novo usuário no sistema
    /// </summary>
    /// <returns>Atributos do usuário</returns>
    /// <response code="201">Usuário cadastrado com sucesso</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<ActionResult> Create([FromBody] User user)
    {
        var validationResult = await _userValidator.ValidateAsync(user);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var resposta = await _userService.Create(user);

        if (resposta is null)
            return BadRequest("Usuário já cadastrado!");
        
        return CreatedAtAction(nameof(GetById), new {id = user.Id}, user);
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <returns>Retorna atributos do usuário e Token</returns>
    /// <response code="200">Usuário logado com sucesso</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [AllowAnonymous]
    [HttpPost("logar")]
    public async Task<ActionResult> Autenticar([FromBody] UserLogin userLogin)
    {
        var resposta = await _authService.Autenticar(userLogin);
        
        if (resposta is null)
            return Unauthorized("Usuário e/ou senha inválidos!");
        
        return Ok(resposta);
    }

    /// <summary>
    /// Atualiza um usuário no sistema
    /// </summary>
    /// <returns>Atributos do usuário</returns>
    /// <response code="201">Usuário cadastrado com sucesso</response>
    /// <response code="500">Erro provavelmente causado pelo Render, tente novamente, por favor</response>
    [Authorize]
    [HttpPut("atualizar/{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] User user)
    {
        user.Id = id;
        
        if (user.Id <= 0)
            return BadRequest("Id do usuário inválido");
        
        var validationResult = await _userValidator.ValidateAsync(user);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var userUpdate = await _userService.GetByEmail(user.Email);

        if (userUpdate is not null && userUpdate.Id != user.Id)
            return BadRequest("O usuário (e-mail) já está em uso por outro usuário!");
        
        var resposta = await _userService.Update(user);
        
        if (resposta == null)
            return NotFound("Usuário não encontrado!");
        
        return Ok(resposta);
    }
}