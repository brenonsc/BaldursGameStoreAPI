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

public class CategoriaControllerTest : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory _factory;
    protected HttpClient _client;
    
    private readonly dynamic token;
    private string Id { get; set; } = string.Empty;
    
    public CategoriaControllerTest(WebAppFactory factory)
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
    public async Task DeveListarTodasAsCategorias()
    {
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.GetAsync("/api/categorias/");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(2)]
    public async Task DeveCriarUmaCategoria()
    {
        var novaCategoria = new Dictionary<string, string>()
        {
            { "tipo", "Simulação" }
        };
        
        var categoriaJson = JsonConvert.SerializeObject(novaCategoria);
        var corpoRequisicao = new StringContent(categoriaJson, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.PostAsync("/api/categorias/", corpoRequisicao);
        resposta.EnsureSuccessStatusCode();
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact, Order(3)]
    public async Task DeveListarUmaCategoria()
    {
        _client.SetFakeBearerToken((object) token);
        
        Id = "1";
        
        var resposta = await _client.GetAsync($"/api/categorias/{Id}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(4)]
    public async Task DeveAtualizarUmaCategoria()
    {
        var novaCategoria = new Dictionary<string, string>()
        {
            { "tipo", "Corrid" }
        };
        
        var categoriaJson = JsonConvert.SerializeObject(novaCategoria);
        var corpoRequisicao = new StringContent(categoriaJson, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.PostAsync("/api/categorias/", corpoRequisicao);
        
        var corpoRespostaPost = await resposta.Content.ReadFromJsonAsync<Categoria>();
        
        if(corpoRespostaPost != null)
            Id = corpoRespostaPost.Id.ToString();

        var categoriaAtualizada = new Dictionary<string, string>()
        {
            { "id", Id },
            { "tipo", "Corrida" }
        };
        
        var categoriaJsonAtualizada = JsonConvert.SerializeObject(categoriaAtualizada);
        var corpoRequisicaoAtualizado = new StringContent(categoriaJsonAtualizada, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var respostaPut = await _client.PutAsync("/api/categorias/", corpoRequisicaoAtualizado);
        
        respostaPut.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(5)]
    public async Task DeveDeletarUmaCategoria()
    {
        _client.SetFakeBearerToken((object) token);
        
        Id = "1";
        
        var resposta = await _client.DeleteAsync($"/api/categorias/{Id}");

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}