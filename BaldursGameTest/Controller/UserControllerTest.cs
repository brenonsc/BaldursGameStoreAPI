using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using BaldursGame.Model;
using BaldursGameTest.Factory;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Extensions.Ordering;

namespace BaldursGameTest.Controller;

public class UserControllerTest : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory _factory;
    protected HttpClient _client;
    
    private readonly dynamic token;
    private string Id { get; set; } = string.Empty;
    
    public UserControllerTest(WebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        token = GetToken();
    }

    private static dynamic GetToken()
    {
        dynamic data = new ExpandoObject();
        data.sub = "root@root.com";
        return data;
    }

    [Fact, Order(1)]
    public async Task DeveCriarUmUsuario()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Breno" },
            { "email", "brenonsc@gmail.com" },
            { "senha", "12345678" },
            {"foto", "" },
            { "dataNascimento", "1998-03-07"}
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/api/usuarios/cadastrar", corpoRequisicao);
        resposta.EnsureSuccessStatusCode();
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact, Order(2)]
    public async Task DeveDarErroEmail()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Breno" },
            { "email", "brenoemail.com" },
            { "senha", "12345678" },
            { "foto", "" },
            { "dataNascimento", "1998-03-07"}
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/api/usuarios/cadastrar", corpoRequisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact, Order(3)]
    public async Task NaoDeveCriarUsuarioDuplicado()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Teste" },
            { "usuario", "teste@email.com" },
            { "senha", "12345678" },
            { "foto", "" },
            { "dataNascimento", "2000-01-01"}
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        await _client.PostAsync("/api/usuarios/cadastrar", corpoRequisicao);
        
        var resposta = await _client.PostAsync("api/usuarios/cadastrar", corpoRequisicao);
        
        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact, Order(4)]
    public async Task DeveListarTodosUsuarios()
    {
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.GetAsync("/api/usuarios/all");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(5)]
    public async Task DeveAtualizarUmUsuario()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Carlos" },
            { "email", "carlos@email.com" },
            { "senha", "12345678" },
            { "foto", "" },
            { "dataNascimento", "2000-01-01"}
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/api/usuarios/cadastrar", corpoRequisicao);
        
        var corpoRespostaPost = await resposta.Content.ReadFromJsonAsync<User>();
        
        if(corpoRespostaPost != null)
            Id = corpoRespostaPost.Id.ToString();

        var usuarioAtualizado = new Dictionary<string, string>()
        {
            { "id", Id },
            { "nome", "Carlos Atualizado" },
            { "email", "carlos@email.com" },
            { "senha", "87654321" },
            { "foto", "" },
            { "dataNascimento", "2000-01-01"}
        };
        
        var usuarioJsonAtualizado = JsonConvert.SerializeObject(usuarioAtualizado);
        var corpoRequisicaoAtualizado = new StringContent(usuarioJsonAtualizado, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var respostaPut = await _client.PutAsync("/api/usuarios/atualizar", corpoRequisicaoAtualizado);
        
        respostaPut.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(6)]
    public async Task DeveListarUmUsuario()
    {
        _client.SetFakeBearerToken((object) token);
        
        Id = "1";
        
        var resposta = await _client.GetAsync($"/api/usuarios/{Id}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Order(7)]
    public async Task DeveAutenticarUmUsuario()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "email", "brenonsc@gmail.com" },
            { "senha", "12345678" }
        };

        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

        var resposta = await _client.PostAsync("/api/usuarios/logar", corpoRequisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}