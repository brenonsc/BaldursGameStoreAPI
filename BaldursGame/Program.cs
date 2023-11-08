using System.Text;
using BaldursGame.Configuration;
using BaldursGame.Data;
using BaldursGame.Filters;
using BaldursGame.Model;
using BaldursGame.Security;
using BaldursGame.Security.Implements;
using BaldursGame.Service;
using BaldursGame.Service.Implements;
using BaldursGame.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BaldursGame;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        
        //Conexão com o banco de dados
        if (builder.Configuration["Environment:Start"] == "PROD")
        {
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
            var connectionString = builder.Configuration.GetConnectionString("ProdConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        }
        else
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }
        
        //Registrar validação das entidades
        builder.Services.AddTransient<IValidator<Produto>, ProdutoValidator>();
        builder.Services.AddTransient<IValidator<Categoria>, CategoriaValidator>();
        builder.Services.AddTransient<IValidator<User>, UserValidator>();
        
        //Registrar as classes de serviço
        builder.Services.AddScoped<IProdutoService, ProdutoService>();
        builder.Services.AddScoped<ICategoriaService, CategoriaService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        //Registrar as classes de segurança
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(Settings.Secret);
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            //Informações do projeto e do desenvolvedor
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Baldur's Game Store API",
                Description = "API REST criada com o ASP.NET Core 7.0 para simular uma API de uma loja de games",
                Contact = new OpenApiContact
                {
                    Name = "Meu LinkedIn",
                    Url = new Uri("https://www.linkedin.com/in/brenonsc/")
                },
                License = new OpenApiLicense
                {
                    Name = "Link do Repositório no Github",
                    Url = new Uri("https://github.com/brenonsc/BaldursGameStoreAPI")
                }
            });

            var filePath = Path.Combine(AppContext.BaseDirectory, "BaldursGame.xml");
            options.IncludeXmlComments(filePath);
            
            options.SchemaFilter<SwaggerSkipPropertyFilter>();

            //Configuração de segurança no Swagger
            options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Digite um token JWT válido",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            //Adicionar a indicação do Endpoint protegido
            options.OperationFilter<AuthResponsesOperationFilter>();
        });
        
        //Adicionar o FluentValidation no Swagger
        builder.Services.AddFluentValidationRulesToSwagger();
        
        //Configuração do CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "MyPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
        });

        var app = builder.Build();
        
        //Criar o banco de dados e as tabelas
        using (var scope = app.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }
        
        app.UseSwagger();

        //Swagger como página inicial na nuvem
        if (app.Environment.IsProduction())
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BaldursGameStore - v1");
                options.RoutePrefix = string.Empty;
            });
        }
        else
        {
            app.UseSwaggerUI();
        }

        app.UseCors("MyPolicy");
        
        app.UseAuthentication();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}

