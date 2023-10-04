
using System.Text;
using BaldursGame.Data;
using BaldursGame.Model;
using BaldursGame.Security;
using BaldursGame.Security.Implements;
using BaldursGame.Service;
using BaldursGame.Service.Implements;
using BaldursGame.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        
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
        builder.Services.AddSwaggerGen();
        
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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("MyPolicy");
        
        app.UseAuthentication();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}

