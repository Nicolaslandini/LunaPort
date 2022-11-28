using Microsoft.EntityFrameworkCore;

namespace LunaPort.Models
{
    public class LunaDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer("Data Source=NICOLASLANDINI;initial catalog=Luna; Integrated Security=true; TrustServerCertificate=True ");
        }

        public DbSet<Estadio> Estadios { get; set; }
        public DbSet<Evento> Eventos { get; set; }


    }
}
