using Microsoft.EntityFrameworkCore;

namespace EstufaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leitura> Leituras { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
    }
}