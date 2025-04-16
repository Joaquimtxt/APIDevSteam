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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        //Tabelas
    }
}
