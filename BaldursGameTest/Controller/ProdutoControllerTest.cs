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

public class ProdutoControllerTest : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory _factory;
    protected HttpClient _client;
    
    private readonly dynamic token;
    private string Id { get; set; } = string.Empty;
    
    public ProdutoControllerTest(WebAppFactory factory)
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
    public async Task DeveListarTodosOsProdutos()
    {
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.GetAsync("/api/produtos/");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Order(2)]
    public async Task DeveCriarUmProduto()
    {
        var novoProduto = new Dictionary<string, string>()
        {
            { "titulo", "Microsoft Flight Simulator" },
            { "descricao", "De aeronaves pequenas a grandes jatos, pilote aeronaves altamente detalhadas e deslumbrantes em um mundo incrivelmente realista. Crie seu plano de voo e vá para qualquer lugar no planeta. Aprecie o voo diurno ou noturno e enfrente condições meteorológicas realistas e desafiadoras." }, 
            { "console", "Xbox Series" },
            { "datalancamento", "2023-04-28"},
            { "preco", "429.00" },
            { "imagem", "https://http2.mlstatic.com/D_NQ_NP_812150-MLB70018720194_062023-O.webp" },
            { "categoriaId", "1" }
        };
        
        var produtoJson = JsonConvert.SerializeObject(novoProduto);
        var corpoRequisicao = new StringContent(produtoJson, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.PostAsync("/api/produtos/", corpoRequisicao);
        resposta.EnsureSuccessStatusCode();
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact, Order(3)]
    public async Task DeveListarUmProduto()
    {
        _client.SetFakeBearerToken((object) token);
        
        Id = "1";
        
        var resposta = await _client.GetAsync($"/api/produtos/{Id}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(4)]
    public async Task DeveAtualizarUmProduto()
    {
        var novoProduto = new Dictionary<string, string>()
        {
            { "titulo", "Baldur's Gate 3" },
            { "descricao", "Se aventure, saqueie, lute e engate romances conforme faz sua jornada pelos Reinos Esquecidos e além. Jogue sozinho e escolha seus companheiros cuidadosamente ou se junte em um grupo de até quatro jogadores em multiplayer." }, 
            { "console", "PS5" },
            { "datalancamento", "2023-08-03"},
            { "preco", "199.00" },
            { "imagem", "https://www.jogodigital.com/image/cache/catalog/capas-de-jogos/ps5/baldurs-gate-3-ps5-1000x1000.jpg" },
            { "categoriaId", "1" }
        };
        
        var produtoJson = JsonConvert.SerializeObject(novoProduto);
        var corpoRequisicao = new StringContent(produtoJson, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.PostAsync("/api/produtos/", corpoRequisicao);
        
        var corpoRespostaPost = await resposta.Content.ReadFromJsonAsync<Produto>();
        
        if(corpoRespostaPost != null)
            Id = corpoRespostaPost.Id.ToString();

        var produtoAtualizado = new Dictionary<string, string>()
        {
            { "id", Id },
            { "titulo", "Baldur's Gate 3" },
            { "descricao", "Se aventure, saqueie, lute e engate romances conforme faz sua jornada pelos Reinos Esquecidos e além. Jogue sozinho e escolha seus companheiros cuidadosamente ou se junte em um grupo de até quatro jogadores em multiplayer." }, 
            { "console", "PlayStation 5" },
            { "datalancamento", "2023-08-03"},
            { "preco", "299.00" },
            { "imagem", "https://www.jogodigital.com/image/cache/catalog/capas-de-jogos/ps5/baldurs-gate-3-ps5-1000x1000.jpg" },
            { "categoriaId", "1" }
        };
        
        var produtoJsonAtualizado = JsonConvert.SerializeObject(produtoAtualizado);
        var corpoRequisicaoAtualizado = new StringContent(produtoJsonAtualizado, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var respostaPut = await _client.PutAsync("/api/produtos/", corpoRequisicaoAtualizado);
        
        respostaPut.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(5)]
    public async Task DeveListarUmProdutoPorTitulo()
    {
        _client.SetFakeBearerToken((object) token);
        
        var titulo = "Baldur's Gate 3";
        
        var resposta = await _client.GetAsync($"/api/produtos/titulo/{titulo}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(6)]
    public async Task DeveListarUmProdutoPorPreco()
    {
        _client.SetFakeBearerToken((object) token);
        
        var min = 100;
        var max = 300;
        
        var resposta = await _client.GetAsync($"/api/produtos/preco/{min}/{max}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(7)]
    public async Task DeveListarUmProdutoPorTituloOuConsole()
    {
        _client.SetFakeBearerToken((object) token);
        
        var titulo = "Baldur's Gate 3";
        var console = "PlayStation 5";
        
        var resposta = await _client.GetAsync($"/api/produtos/titulo/{titulo}/ouconsole/{console}");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Order(8)]
    public async Task NaoDeveCadastrarProdutoPorDescricaoComMuitosCaracteres()
    {
        var novoProduto = new Dictionary<string, string>()
        {
            { "titulo", "The Last of Us Part II" },
            { "descricao", "Cinco anos depois de uma jornada perigosa pelos Estados Unidos num cenário pós-pandêmico, Ellie e Joel se acomodaram em Jackson, Wyoming. A vida numa comunidade próspera de sobreviventes lhes trouxe paz e estabilidade, apesar da ameaça constante dos infectados e de outros sobreviventes mais desesperados.Quando um evento violento interrompe a paz, Ellie parte numa jornada incansável para fazer justiça e virar a página. Enquanto vai atrás de cada um dos responsáveis, ela se confronta com as repercussões físicas e emocionais devastadoras das próprias ações." }, 
            { "console", "PlayStation 4" },
            { "datalancamento", "2020-06-19"},
            { "preco", "199.00" },
            { "imagem", "https://i.zst.com.br/thumbs/12/29/15/1017896850.jpg" },
            { "categoriaId", "1" }
        };
        
        var produtoJson = JsonConvert.SerializeObject(novoProduto);
        var corpoRequisicao = new StringContent(produtoJson, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.PostAsync("/api/produtos/", corpoRequisicao);
        
        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact, Order(9)]
    public async Task DeveDeletarUmProduto()
    {
        _client.SetFakeBearerToken((object) token);
        
        Id = "1";
        
        var resposta = await _client.DeleteAsync($"/api/produtos/{Id}");

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}