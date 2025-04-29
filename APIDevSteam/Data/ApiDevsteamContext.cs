using APIDevSteam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIDevSteam.Data
{
    public class ApiDevsteamContext : IdentityDbContext<Usuario>
    {
        public ApiDevsteamContext(DbContextOptions<ApiDevsteamContext> options)
            : base(options)
        {
        }

        //DbSet
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<JogoCategoria> JogosCategorias { get; set; }
        public DbSet<JogoMidia> JogosMidias { get; set; }

        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<ItemCarrinho> ItensCarrinhos { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<CupomCarrinho> CuponsCarrinhos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Tabelas
            modelBuilder.Entity<Jogo>().ToTable("Jogos");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<JogoCategoria>().ToTable("JogosCategorias");
            modelBuilder.Entity<JogoMidia>().ToTable("JogosMidias");
            modelBuilder.Entity<Carrinho>().ToTable("Carrinhos");
            modelBuilder.Entity<ItemCarrinho>().ToTable("ItensCarrinhos");
            modelBuilder.Entity<Cupom>().ToTable("Cupons");
            modelBuilder.Entity<CupomCarrinho>().ToTable("CuponsCarrinhos");

        }


    }
}
