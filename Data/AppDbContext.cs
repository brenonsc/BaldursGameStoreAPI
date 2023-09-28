using BaldursGame.Model;
using Microsoft.EntityFrameworkCore;

namespace BaldursGame.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().ToTable("tb_produtos");
        modelBuilder.Entity<Categoria>().ToTable("tb_categorias");

        _ = modelBuilder.Entity<Produto>()
            .HasOne(_ => _.Categoria)
            .WithMany(t => t!.Produto)
            .HasForeignKey("CategoriaId")
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    //Registrar um DbSet para cada entidade - Objeto responsável por manipular as tabelas
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;
}