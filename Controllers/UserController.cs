using BaldursGame.Model;
using BaldursGame.Security;
using BaldursGame.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaldursGame.Controllers;

[Route("~/usuarios")]
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
    
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _userService.GetAll());
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var user = await _userService.GetById(id);
            
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

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

    [AllowAnonymous]
    [HttpPost("logar")]
    public async Task<ActionResult> Autenticar([FromBody] UserLogin userLogin)
    {
        var resposta = await _authService.Autenticar(userLogin);
        
        if (resposta is null)
            return Unauthorized("Usuário e/ou senha inválidos!");
        
        return Ok(resposta);
    }

    [Authorize]
    [HttpPut("atualizar")]
    public async Task<ActionResult> Update([FromBody] User user)
    {
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